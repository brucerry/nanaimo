/* Exercise the final-frame rectangle and input mapping without a GPU or saves. */
#include <stdio.h>
#include "display.c"
static RECT captured;
static RECT *capturedPointer;
static LPARAM mousePosition;
static HRESULT WINAPI CaptureBlt(void *surface, RECT *destination, void *source, RECT *sourceRect, DWORD flags, void *fx)
{
    capturedPointer = destination;
    if (destination) captured = *destination;
    return S_OK;
}
static LRESULT CALLBACK CaptureInput(HWND window, UINT message, WPARAM wp, LPARAM lp)
{
    mousePosition = lp;
    return 0;
}
int main(void)
{
    int sizes[][2] = {{800,600},{1024,768},{1280,720},{1280,800},{1366,768},
        {1440,900},{1600,900},{1920,1080},{1920,1200},{2560,1440},{3840,2160}};
    int i;
    RECT destination = {0,0,800,600}, source = {0,0,800,600};
    gameWindow = CreateWindowExA(0, "STATIC", "Display mapping test", WS_POPUP,
        0,0,800,600,NULL,NULL,GetModuleHandle(NULL),NULL);
    if (!gameWindow) return 1;
    primarySurface = (void *)1;
    originalBlt = CaptureBlt;
    originalProc = CaptureInput;
    for (i=0;i<sizeof(sizes)/sizeof(sizes[0]);i++) {
        int width=sizes[i][0], height=sizes[i][1];
        SetWindowPos(gameWindow,NULL,0,0,width,height,SWP_NOZORDER);
        ScaledBlt(primarySurface,&destination,(void *)2,&source,0,NULL);
        if (captured.right-captured.left != width || captured.bottom-captured.top != height) return 2;
        GameProc(gameWindow,WM_MOUSEMOVE,0,MAKELPARAM(width-1,height-1));
        if (LOWORD(mousePosition)!=799 || HIWORD(mousePosition)!=599) return 3;
        GameProc(gameWindow,WM_LBUTTONDOWN,0,MAKELPARAM(width/2,height/2));
        if (LOWORD(mousePosition)!=400 || HIWORD(mousePosition)!=300) return 4;
        GameProc(gameWindow,WM_MOUSEWHEEL,0,MAKELPARAM(width-1,height-1));
        if (LOWORD(mousePosition)!=width-1 || HIWORD(mousePosition)!=height-1) return 5;
        ScaledBlt((void *)3,&destination,(void *)2,&source,0,NULL);
        if (capturedPointer != &destination) return 6;
        ScaledBlt(primarySurface,NULL,(void *)2,&source,0,NULL);
        if (capturedPointer) return 7;
        printf("DISPLAY_MAPPING_PASS %dx%d\n",width,height);
    }
    DestroyWindow(gameWindow);
    return 0;
}
