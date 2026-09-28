using FlightIslandServer.Desktop.Models;

namespace FlightIslandServer.Desktop.Services;

internal static class VillageBotLanguageCatalog
{
    private static readonly string[] IdlePhrases =
    [
        "今日條村入面幾熱鬧的。", "先喺呢度唞下一陣。", "有人準備去地宮咩？", "啱啱見到一隻很可愛嘅寵物。",
        "慢慢逛，總能發現新嘢。", "今日也要記得完成任務。", "我嘅寵物又升了一級。", "商城裏好像上了新衣服。",
        "有人一齊刷卡片咩？", "地宮組隊嘅話會輕鬆很多。", "站一陣再出發。", "呢個條村嘅風景真係唔錯。",
        "啱啱整理完寵物箱。", "卡片冊還差好幾張呢。", "準備去睇下下一扇門後面有乜嘢。", "升級技能也要唔少SP。",
        "有人要組隊就喊我。", "今日的運氣應該唔錯。", "先逛逛再去打怪。", "換套衫，心情都會變好。",
        "寵物跟在身邊很有安全感。", "聽說高分結算獎勵更好。", "唔好唔記得及時保存公寓裝修。", "條村入面到處走走也挺有意思。"
    ];

    public static IReadOnlyList<VillageBotPhraseRecord> Entries { get; } =
    [
        new() { Category = "Idle speech", Trigger = "Random", Text = "Select a village conversation line at the configured interval." },
        new() { Category = "Greeting", Trigger = "你好 / 嗨 / hello", Text = "你好呀，今日準備去邊度玩？" },
        new() { Category = "Presence", Trigger = "喺唔喺度 / 有人喺度", Text = "喺度呀，我而家喺度條村入面逛逛。" },
        new() { Category = "Identity", Trigger = "名字 / 你係邊個", Text = "我係條村入面嘅飛行員。" },
        new() { Category = "Pets", Trigger = "寵物", Text = "寵物可以在寵物箱裏更換，記得帶佢一齊冒險。" },
        new() { Category = "Dungeons", Trigger = "地宮 / 組隊 / BOSS", Text = "組隊進入地宮會更穩，打完記得看結算獎勵。" },
        new() { Category = "Cards", Trigger = "卡片 / 卡片冊", Text = "卡片可以通過地宮戰鬥和結算攞到。" },
        new() { Category = "Shop", Trigger = "商城 / 買東西", Text = "購買後可以去對應的衣物箱、寵物箱或道具欄睇下。" },
        new() { Category = "Skills", Trigger = "技能 / SP", Text = "SP可以用來升級技能，裝好技能再進入戰鬥。" },
        new() { Category = "Level", Trigger = "等級 / 升級", Text = "完成任務和地宮戰鬥都能積累經驗。" },
        new() { Category = "Thanks", Trigger = "唔該 / 多謝", Text = "唔使客氣，祝你玩得開心。" },
        new() { Category = "Farewell", Trigger = "拜拜 / 拜拜", Text = "拜拜，路上小心。" },
        new() { Category = "Fallback", Trigger = "Bot name or general question", Text = "呢個我都在研究，要唔好一齊去條村入面睇下？" }
    ];

    public static string CreateIdlePhrase(IReadOnlyCollection<string>? recentMessages = null)
        => Pick(string.Empty, recentMessages, IdlePhrases);

    public static string CreateReply(
        string playerName,
        string message,
        IReadOnlyCollection<string>? recentMessages = null)
    {
        var prefix = string.IsNullOrWhiteSpace(playerName) ? string.Empty : $"{playerName}，";
        if (ContainsAny(message, "你好", "嗨", "哈嘍", "hello", "hi"))
            return Pick(prefix, recentMessages,
                "你好呀，今日準備去邊度玩？",
                "嗨，啱啱在條村碰見你。",
                "你好，今日也一齊四處逛逛吧。",
                "哈嘍，我正準備在條村轉一圈。");
        if (ContainsAny(message, "喺唔喺度", "有人喺度"))
            return Pick(prefix, recentMessages,
                "喺度呀，我而家喺度條村入面逛逛。",
                "我喺度，啱啱停下來唞下一陣。",
                "喺度呀，你想去邊度？",
                "喺度呀度呢，正好未行遠。");
        if (ContainsAny(message, "名字", "你係邊個"))
            return Pick(prefix, recentMessages,
                "我係條村入面嘅飛行員。",
                "我都係來呢度冒險的飛行員。",
                "在條村入面經常能見到我。",
                "叫我嘅名字就可以搵到我啦。");
        if (message.Contains("寵物", StringComparison.OrdinalIgnoreCase))
            return Pick(prefix, recentMessages,
                "寵物可以在寵物箱裏更換，記得帶佢一齊冒險。",
                "換好寵物再出發，戰鬥時會輕鬆一些。",
                "我都常去寵物箱睇下有冇合適的夥伴。",
                "帶上喜歡嘅寵物，在條村入面都可以見到它跟隨。");
        if (ContainsAny(message, "地宮", "組隊", "boss"))
            return Pick(prefix, recentMessages,
                "組隊進地宮會更穩，打完記得看結算獎勵。",
                "地宮最好找幾個人一齊去，路上能互相照應。",
                "打BOSS前先準備好寵物和技能吧。",
                "想組隊嘅話可以先開房間等其他人加入。");
        if (ContainsAny(message, "卡片", "卡冊"))
            return Pick(prefix, recentMessages,
                "卡片可以通過地宮戰鬥和結算攞到。",
                "我都在慢慢收集卡片冊裏缺少嘅卡片。",
                "通關結算時記得睇下拿到了乜嘢卡片。",
                "有些卡片要多刷幾次地宮才容易遇到。");
        if (ContainsAny(message, "商城", "商場", "買東西"))
            return Pick(prefix, recentMessages,
                "購買後可以去對應嘅物品欄睇下。",
                "買完記得去衣物箱、寵物箱或道具欄找找。",
                "商城入面嘅東西會放進對應分類的揹包。",
                "先睇下物品類型，買完就知該去哪個箱子找了。");
        if (ContainsAny(message, "技能", "sp"))
            return Pick(prefix, recentMessages,
                "SP可以升級技能，裝好技能再進入戰鬥。",
                "技能升級以後，打地宮會順手唔少。",
                "唔好唔記得把技能放進技能欄再出發。",
                "有SP嘅話可以先睇下哪些技能值得升級。");
        if (ContainsAny(message, "等級", "升級", "經驗"))
            return Pick(prefix, recentMessages,
                "完成任務和地宮戰鬥都能積累經驗。",
                "多做任務，等級會升得更穩定。",
                "地宮通關都可以拿到唔少經驗。",
                "慢慢玩就會升級，唔使一直趕進度。");
        if (ContainsAny(message, "唔該", "多謝"))
            return Pick(prefix, recentMessages,
                "唔使客氣，祝你玩得開心。",
                "沒事，能幫上忙就好。",
                "唔使謝，路上小心。",
                "客氣啦，下次條村見。");
        if (ContainsAny(message, "拜拜", "拜拜", "下次見"))
            return Pick(prefix, recentMessages,
                "拜拜，路上小心。",
                "拜拜，下次條村拜拜。",
                "回頭見，我再逛一陣。",
                "下次見，祝你冒險順利。");
        return Pick(prefix, recentMessages,
            "呢個我都在研究，要唔好一齊去條村入面睇下？",
            "我還不太確定，等我再四處睇下。",
            "呢個問題挺有意思，我都想弄清楚。",
            "要不先去附近轉轉，也許能搵到答案。");
    }

    private static string Pick(
        string prefix,
        IReadOnlyCollection<string>? recentMessages,
        params string[] candidates)
    {
        var available = recentMessages is null || recentMessages.Count == 0
            ? candidates
            : candidates.Where(candidate => !recentMessages.Any(message =>
                message.EndsWith(candidate, StringComparison.Ordinal))).ToArray();
        var pool = available.Length > 0 ? available : candidates;
        return prefix + pool[Random.Shared.Next(pool.Length)];
    }

    private static bool ContainsAny(string value, params string[] terms)
        => terms.Any(term => value.Contains(term, StringComparison.OrdinalIgnoreCase));
}
