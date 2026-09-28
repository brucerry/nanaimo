using System.ComponentModel;
using System.Data;
using System.Text.Json;
using FlightIslandServer.Desktop.Models;
using FlightIslandServer.Desktop.Services;

namespace Nanaimo.Launcher;

internal sealed class GmManagementControl : UserControl
{
    private readonly string dataDirectory;
    private readonly ComboBox accounts = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 280, AutoCompleteSource=AutoCompleteSource.ListItems, AutoCompleteMode=AutoCompleteMode.SuggestAppend };
    private readonly ListBox navigation = new() { Dock = DockStyle.Fill, IntegralHeight = false, Name="gmNavigation", AccessibleName="GM sections" };
    private readonly Panel content = new() { Dock = DockStyle.Fill };
    private readonly Label status = new() { AutoSize = true, Padding = new Padding(8, 7, 0, 0) };
    private readonly Button refresh = new() { Text = "重新整理", AutoSize = true };
    private bool loading, dirty;
    private long loadedAccount;
    private int loadedView;
    private IReadOnlyList<AccountRecord> accountRecords = [];
    private CharacterRecord? character;
    private GmCharacterEdit? edit;
    private IReadOnlyList<GmCatalogItem>? catalog;
    private sealed record AccountChoice(long Id, string Label) { public override string ToString() => Label; }
    private long AccountId => (accounts.SelectedItem as AccountChoice)?.Id ?? 0;
    private DatabaseService Database => new(dataDirectory);
    internal bool IsBusy => loading;
    internal void ResetAfterClear()
    {
        loading = true;
        try
        {
            accounts.Items.Clear(); accountRecords = []; character = null; edit = null;
            dirty = false; loadedAccount = 0;
            foreach (Control child in content.Controls.Cast<Control>().ToArray()) child.Dispose();
            status.Text = "仲未有帳號";
        }
        finally { loading = false; }
    }

    internal GmManagementControl(string root)
    {
        dataDirectory = Path.Combine(root, "server-merged", "data");
        Dock = DockStyle.Fill;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
        toolbar.Controls.AddRange([new Label { Text = "帳號／角色", AutoSize = true, Padding = new Padding(0,7,0,0) }, accounts, refresh, status]);
        var split = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185)); split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
        navigation.Items.AddRange(new object[] { "角色／貨幣", "背包", "卡片收藏", "技能", "任務", "小屋", "商店願望清單",
            "副本進度", "師徒", "好友", "好友申請", "卡片交易", "連線", "副本", "天空競技場", "隊伍", "交易", "操作記錄" });
        split.Controls.Add(navigation,0,0); split.Controls.Add(content,1,0);
        layout.Controls.Add(toolbar,0,0); layout.Controls.Add(split,0,1); Controls.Add(layout);
        refresh.Click += async (_,_) => { if (DiscardChanges()) await RunAsync(ReloadAccountsAsync); };
        accounts.SelectedIndexChanged += async (_,_) => await SelectionChangedAsync();
        navigation.SelectedIndexChanged += async (_,_) => await SelectionChangedAsync();
        VisibleChanged += async (_,_) => { if (Visible && accounts.Items.Count == 0) await RunAsync(ReloadAccountsAsync); };
    }

    private async Task SelectionChangedAsync()
    {
        if(loading)return;
        if(DiscardChanges()){await RunAsync(LoadViewAsync);return;}
        loading=true;
        try
        {
            accounts.SelectedIndex=accounts.Items.Cast<AccountChoice>().ToList().FindIndex(a=>a.Id==loadedAccount);
            navigation.SelectedIndex=loadedView;
        }
        finally{loading=false;}
    }

    private bool DiscardChanges()
    {
        if (!dirty) return true;
        if (MessageBox.Show(this,"真係要放棄仲未儲存的角色變更？","管理員工具",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes) return false;
        dirty=false; return true;
    }

    private async Task RunAsync(Func<Task> action)
    {
        if (loading) return;
        loading=true; accounts.Enabled=false; navigation.Enabled=false; refresh.Enabled=false; content.Enabled=false;
        try { await action(); }
        catch (Exception ex) { status.Text="操作失敗"; MessageBox.Show(this,ex.Message,"管理員工具",MessageBoxButtons.OK,MessageBoxIcon.Error); }
        finally { loading=false; accounts.Enabled=true; navigation.Enabled=true; refresh.Enabled=true; content.Enabled=true; }
    }

    private async Task ReloadAccountsAsync()
    {
        long previous=AccountId;
        accountRecords=await Database.GetAccountsAsync();
        accounts.Items.Clear();
        foreach (var a in accountRecords) accounts.Items.Add(new AccountChoice(a.Id,$"{a.Username} / {a.CharacterName ?? "尚未建立角色"}"));
        accounts.SelectedIndex=accounts.Items.Cast<AccountChoice>().ToList().FindIndex(a=>a.Id==previous);
        if (accounts.SelectedIndex<0 && accounts.Items.Count>0) accounts.SelectedIndex=0;
        if (navigation.SelectedIndex<0) navigation.SelectedIndex=0;
        await LoadViewAsync();
    }

    private async Task LoadViewAsync()
    {
        loadedAccount=AccountId; loadedView=navigation.SelectedIndex;
        foreach (Control old in content.Controls.Cast<Control>().ToArray()) old.Dispose();
        character=AccountId>0 ? await Database.GetCharacterAsync(AccountId) : null;
        status.Text=character is null ? "仲未有角色" : $"{character.Name}  |  {(character.IsOnline ? "上線" : "離線")}  |  等級 {character.Level}";
        dirty=false;
        int view=navigation.SelectedIndex;
        if (view==0) { ShowCharacter(); return; }
        if (view is >=1 and <=3) { await ShowStockAsync(view==1 ? "item" : view==2 ? "card" : "skill"); return; }
        if (view==4) { await ShowQuestsAsync(); return; }
        if (view==5) { await ShowTableAsync(async()=> await Database.GetApartmentPlacementsByAccountAsync(AccountId)); return; }
        if (view==6) { await ShowTableAsync(async()=> await Database.GetShopWishlistsForGmAsync(AccountId)); return; }
        if (view==7) { await ShowTableAsync(async()=> character is null ? [] : await Database.GetDungeonProgressAdminAsync(character.Id)); return; }
        if (view==8) { await ShowTableAsync(async()=>await Database.GetMentorAdvertisementsForAdminAsync()); return; }
        if (view==9) { await ShowTableAsync(async()=>await Database.GetFriendRelationsForAdminAsync(), "刪除好友關係", async row=>
            { if (!await Database.DeleteFriendRelationForAdminAsync(Convert.ToInt64(row["Id"]))) throw new InvalidOperationException("此好友關係已唔存在。"); }); return; }
        if (view==10) { await ShowTableAsync(async()=>await Database.GetFriendRequestsForAdminAsync(), "刪除申請", async row=>
            { if (!await Database.DeleteFriendRequestForAdminAsync(Convert.ToInt64(row["SerialNo"]))) throw new InvalidOperationException("此申請已唔存在。"); }); return; }
        if (view==11) { await ShowTableAsync(async()=>await Database.GetAuctionListingsForAdminAsync()); return; }
        if (view is >=12 and <=16) { await ShowRuntimeAsync(view); return; }
        await ShowTableAsync(async()=>await Database.GetGmAuditAsync());
    }

    private void ShowCharacter()
    {
        if (character is null) return;
        var account=accountRecords.First(a=>a.Id==AccountId);
        edit=GmCharacterEdit.From(character,account);
        var property=new PropertyGrid { Dock=DockStyle.Fill, ToolbarVisible=false, HelpVisible=false, SelectedObject=edit, PropertySort=PropertySort.Categorized };
        property.PropertyValueChanged+=(_,_)=> { dirty=true; property.Refresh(); };
        var save=new Button { Text="儲存角色", AutoSize=true, Enabled=!character.IsOnline };
        save.Click+=async(_,_)=>await RunAsync(async()=>
        {
            property.Focus(); await Database.SaveGmCharacterAsync(edit); dirty=false;
            await ReloadAccountsAsync(); status.Text+="  已儲存";
        });
        AddLayout(property,save);
    }

    private async Task ShowStockAsync(string kind)
    {
        catalog ??= await Task.Run(DatabaseService.GetGmCatalog);
        var owned=new Dictionary<uint,int>();
        if (character is not null)
        {
            if (kind=="item") foreach(var i in character.Items) owned[i.ItemCode]=i.Quantity;
            if (kind=="card") foreach(var i in await Database.GetCharacterCardsAsync(character.Id)) owned[i.CardCode]=i.Quantity;
            if (kind=="skill") foreach(var i in await Database.GetCharacterSkillsAsync(character.Id)) owned[i.SkillCode]=i.Grade;
        }
        var grid=Grid();
        grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
        var search=new TextBox { Width=155, PlaceholderText="名稱或編號" };
        var onlyOwned=new CheckBox { Text="僅顯示已擁有", AutoSize=true, Padding=new Padding(0,5,0,0) };
        var number=new NumericUpDown { Minimum=kind=="skill" ? 0 : 1, Maximum=kind=="skill" ? 5 : kind=="card" ? 255 : 65535, Value=1, Width=65 };
        var add=new Button { Text=kind=="skill" ? "設定等級" : "發放", AutoSize=true };
        var remove=new Button { Text="移除", AutoSize=true, Visible=kind!="skill" };
        void Filter()
        {
            string term=search.Text.Trim();
            grid.DataSource=catalog.Where(i=>i.Kind==kind && (!onlyOwned.Checked || owned.GetValueOrDefault(i.Code)>0)
                && (i.Name.Contains(term,StringComparison.OrdinalIgnoreCase)||i.Code.ToString().Contains(term)))
                .Select(i=>new { Code=i.Code, Name=i.Name, Category=i.Category, Owned=owned.GetValueOrDefault(i.Code) }).ToList();
            Translate(grid);
        }
        async Task Change(int sign)
        {
            if (grid.CurrentRow is null) return;
            uint code=Convert.ToUInt32(grid.CurrentRow.Cells["Code"].Value);
            await Database.ChangeGmStockAsync(AccountId,kind,code,sign*(int)number.Value);
            await LoadViewAsync(); status.Text+="  已儲存";
        }
        search.TextChanged+=(_,_)=>Filter(); onlyOwned.CheckedChanged+=(_,_)=>Filter();
        add.Click+=async(_,_)=>await RunAsync(()=>Change(1)); remove.Click+=async(_,_)=>await RunAsync(()=>Change(-1));
        add.Enabled=remove.Enabled=character is { IsOnline:false };
        AddLayout(grid,search,onlyOwned,number,add,remove); Filter();
    }

    private async Task ShowQuestsAsync()
    {
        if (character is null) return;
        var quests=await Database.GetCharacterTasksForAdminAsync(AccountId,character.Id);
        var grid=Grid(); grid.DataSource=quests.Select(CharacterTaskAdminRecord.Create).ToList(); Translate(grid);
        var code=new NumericUpDown { Minimum=1,Maximum=uint.MaxValue,Width=110 };
        var grant=new Button { Text="發放卷軸任務",AutoSize=true };
        var activate=new Button { Text="啟用任務",AutoSize=true };
        var finish=new Button { Text="完成目標",AutoSize=true };
        var remove=new Button { Text="刪除任務",AutoSize=true };
        async Task Act(Func<Task<(bool Success,string Error)>> action)
        { var r=await action(); if(!r.Success) throw new InvalidOperationException(r.Error); await LoadViewAsync(); }
        uint Selected()=>grid.CurrentRow is null ? throw new InvalidOperationException("請選取任務。") : Convert.ToUInt32(grid.CurrentRow.Cells["QuestId"].Value);
        grant.Click+=async(_,_)=>await RunAsync(()=>Act(()=>Database.GrantQuestTaskFromAdminAsync(AccountId,character.Id,(uint)code.Value)));
        activate.Click+=async(_,_)=>await RunAsync(()=>Act(()=>Database.ActivateQuestTaskFromAdminAsync(AccountId,character.Id,Selected())));
        finish.Click+=async(_,_)=>await RunAsync(()=>Act(()=>Database.SetQuestTaskProgressFromAdminAsync(AccountId,character.Id,Selected(),Convert.ToUInt32(grid.CurrentRow!.Cells["RequiredCount"].Value))));
        remove.Click+=async(_,_)=>await RunAsync(()=>Act(()=>Database.DeleteQuestTaskFromAdminAsync(AccountId,character.Id,Selected())));
        foreach(var b in new[]{grant,activate,finish,remove}) b.Enabled=!character.IsOnline;
        AddLayout(grid,code,grant,activate,finish,remove);
    }

    private async Task ShowTableAsync<T>(Func<Task<T>> load, string? actionName=null, Func<DataRow,Task>? action=null)
    {
        var grid=Grid(); var table=JsonTable(JsonSerializer.SerializeToElement(await load())); grid.DataSource=table; Translate(grid);
        if(action is null) { AddLayout(grid); return; }
        var button=new Button { Text=actionName,AutoSize=true };
        button.Click+=async(_,_)=>
        {
            if(grid.CurrentRow?.DataBoundItem is not DataRowView selected) return;
            if(MessageBox.Show(this,$"確定要{actionName}？","管理員工具",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            await RunAsync(async()=>{await action(selected.Row); await LoadViewAsync();});
        };
        AddLayout(grid,button);
    }

    private async Task ShowRuntimeAsync(int view)
    {
        string name=new[]{"connections","native","arena","parties","trades"}[view-12];
        var rows=await GmRuntimeControl.RequestAsync(dataDirectory,"snapshot",name);
        var grid=Grid(); grid.DataSource=JsonTable(rows); Translate(grid);
        if(view!=12){AddLayout(grid);return;}
        var kick=new Button { Text="中斷選取的連線",AutoSize=true };
        kick.Click+=async(_,_)=>await RunAsync(async()=>
        {
            if(grid.CurrentRow is null)return;
            await GmRuntimeControl.RequestAsync(dataDirectory,"kick",Convert.ToString(grid.CurrentRow.Cells["SessionId"].Value) ?? "");
            await LoadViewAsync();
        });
        AddLayout(grid,kick);
    }

    private void AddLayout(Control body, params Control[] commands)
    {
        if(body is DataGridView empty && empty.DataSource is not null && empty.Rows.Count==0)
            empty.Controls.Add(new Label { Text="仲未有資料",AutoSize=true,Location=new Point(12,36),ForeColor=SystemColors.GrayText });
        var layout=new TableLayoutPanel { Dock=DockStyle.Fill,RowCount=2,ColumnCount=1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute,commands.Length==0 ? 0 : 68)); layout.RowStyles.Add(new RowStyle(SizeType.Percent,100));
        var bar=new FlowLayoutPanel { Dock=DockStyle.Fill,AutoScroll=true,WrapContents=true };
        bar.Controls.AddRange(commands); layout.Controls.Add(bar,0,0); layout.Controls.Add(body,0,1); content.Controls.Add(layout);
    }

    private static DataGridView Grid()=>new() { Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AllowUserToDeleteRows=false,RowHeadersVisible=false,
        MultiSelect=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.DisplayedCells,
        BackgroundColor=Color.White,BorderStyle=BorderStyle.None,AutoGenerateColumns=true };

    private static DataTable JsonTable(JsonElement rows)
    {
        var table=new DataTable();
        if(rows.ValueKind!=JsonValueKind.Array)return table;
        foreach(var row in rows.EnumerateArray())
        {
            foreach(var p in row.EnumerateObject())if(!table.Columns.Contains(p.Name))table.Columns.Add(p.Name);
            var next=table.NewRow(); foreach(var p in row.EnumerateObject())next[p.Name]=p.Value.ValueKind switch
            {
                JsonValueKind.String => p.Value.GetString() ?? "",
                JsonValueKind.True => "是",
                JsonValueKind.False => "否",
                _ => p.Value.ToString()
            };
            table.Rows.Add(next);
        }
        return table;
    }

    private static void Translate(DataGridView grid)
    {
        var labels=new Dictionary<string,string> { ["Id"]="編號",["Name"]="名稱",["CharacterId"]="角色編號",["AccountId"]="帳號編號",["CharacterName"]="角色名稱",["Username"]="帳號",
            ["ItemCode"]="道具編號",["QuestId"]="任務編號",["RequiredCount"]="需求數量",["State"]="狀態",["ProgressStatus"]="進度",["ObjectiveSummary"]="目標",["RewardSummary"]="獎勵",
            ["Time"]="時間",["Action"]="操作",["Details"]="詳細資料",["SessionId"]="連線階段",["RemoteIp"]="位址",["ChannelId"]="頻道",["RoomId"]="房間",["Title"]="標題",["OwnerName"]="擁有者",
            ["MemberCount"]="成員",["StateText"]="狀態",["Episode"]="章節",["Difficulty"]="難度",["CreatedAt"]="建立時間",["UpdatedAt"]="更新時間",
            ["Code"]="編號",["Category"]="分類",["Owned"]="已擁有",["SerialNo"]="流水號",["Quantity"]="數量",["Slot"]="欄位",["Page"]="頁面",
            ["IsOnline"]="上線狀態",["Status"]="狀態",["CharacterLevel"]="角色等級",["MapId"]="地圖編號",["X"]="橫座標",["Y"]="縱座標" };
        var additional = new Dictionary<string,string>
        {
            ["AdvertisingStatus"]="招募狀態", ["AllowType"]="允許類型", ["BestElapsedMinutes"]="最佳分鐘數", ["BestElapsedText"]="最佳時間", ["BestRatings"]="最佳評價", ["BestScore"]="最高分數",
            ["BossEnergy"]="首領能量", ["CardsText"]="卡片", ["Categories"]="分類", ["CategoryCode"]="分類編號", ["CategoryName"]="分類名稱", ["ChannelStatus"]="頻道狀態",
            ["ClearMask"]="通關標記", ["ClearedAt"]="通關時間", ["CreatedAtText"]="建立時間", ["CreatedAtUtc"]="建立時間", ["CreatedUtc"]="建立時間", ["Difficulty1Status"]="難度一", ["Difficulty2Status"]="難度二", ["Difficulty3Status"]="難度三",
            ["DifficultyPerformances"]="各難度成績", ["Direction"]="方向", ["Dungeon"]="副本", ["DungeonArchiveDisplay"]="副本紀錄", ["DungeonDisplay"]="副本", ["DungeonStatus"]="副本狀態", ["EpisodeDisplay"]="章節",
            ["FinalCount"]="最終人數", ["FinalText"]="最終狀態", ["FirstCharacterId"]="第一角色編號", ["FirstCharacterName"]="第一角色名稱", ["Friends"]="好友", ["GameType"]="遊戲類型", ["GameTypeText"]="遊戲類型",
            ["Hans"]="金幣", ["HansPerItem"]="單價", ["HeartbeatCount"]="心跳次數", ["HeartbeatText"]="心跳狀態", ["HpMpText"]="生命與魔力", ["InteriorType"]="家具類型", ["InteriorTypeName"]="家具類型",
            ["InviteeName"]="受邀者", ["InviterName"]="邀請者", ["IsAdvertising"]="招募中", ["IsBlocked"]="已封鎖", ["IsSuperBoss"]="超級首領", ["IsWaitingConfirmation"]="等待確認", ["ItemName"]="道具名稱",
            ["JoinOrder"]="加入順序", ["JoinedCount"]="已加入人數", ["JoinedText"]="加入狀態", ["LastHeartbeatUtc"]="最後心跳時間", ["Layer"]="圖層", ["LessonCode"]="課程編號", ["Level"]="等級",
            ["ManagementStatus"]="管理狀態", ["Memo"]="備註", ["Message"]="訊息", ["Mirror"]="鏡像", ["ObjectiveSummary"]="目標摘要", ["OnlineDurationText"]="上線時長", ["OnlineSinceText"]="上線時間", ["OnlineSinceUtc"]="上線時間",
            ["OnlineStatus"]="連線狀態", ["OnlineText"]="連線狀態", ["OriginalQuantity"]="原始數量", ["OwnerCharacterId"]="擁有者角色編號", ["OwnerCharacterName"]="擁有者角色名稱", ["P2PEndpoint"]="點對點位址",
            ["PartyId"]="隊伍編號", ["PendingHans"]="待收金幣", ["PositionStatus"]="位置狀態", ["Price"]="價格", ["Progress2"]="進度二", ["Progress3"]="進度三", ["Property"]="屬性", ["Protocol"]="協定",
            ["ReadyCount"]="就緒人數", ["ReadyText"]="就緒狀態", ["RemainingQuantity"]="剩餘數量", ["RequestOpcode"]="請求代碼", ["RequesteeCharacterId"]="受邀角色編號", ["RequesteeCharacterName"]="受邀角色名稱",
            ["RequesterCharacterId"]="邀請角色編號", ["RequesterCharacterName"]="邀請角色名稱", ["RequesterName"]="申請者", ["RequiredSummary"]="需求摘要", ["RoleText"]="角色", ["RuntimeState"]="執行狀態", ["SceneText"]="場景",
            ["Score"]="分數", ["ScrollCode"]="卷軸編號", ["SecondCharacterId"]="第二角色編號", ["SecondCharacterName"]="第二角色名稱", ["SelectedDifficulty"]="所選難度", ["SellerAccountId"]="賣家帳號編號",
            ["SellerCharacterId"]="賣家角色編號", ["SellerCharacterName"]="賣家角色名稱", ["SellerUsername"]="賣家帳號", ["SlotIndex"]="欄位", ["SnapshotUtc"]="快照時間", ["SoldQuantity"]="已售數量",
            ["StatusName"]="狀態", ["StatusText"]="狀態", ["SupportsProgressManagement"]="可管理進度", ["TargetName"]="目標名稱", ["TargetSummary"]="目標摘要", ["TaskType"]="任務類型", ["TeamCode"]="隊伍代碼",
            ["UniqueNumber"]="唯一編號", ["Unrelated"]="無關", ["UpdatedAtText"]="更新時間", ["UpdatedAtUtc"]="更新時間"
        };
        foreach(var pair in additional) labels[pair.Key] = pair.Value;
        foreach(DataGridViewColumn column in grid.Columns)if(labels.TryGetValue(column.Name,out var label))column.HeaderText=label;
    }
}
