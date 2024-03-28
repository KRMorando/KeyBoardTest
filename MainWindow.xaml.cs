using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace KeyBoardTest {
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window {
        const int VK_PROCESSKEY = 0xE5;
        const int WM_IME_COMPOSITION = 0x10F;
        const int WM_IME_ENDCOMPOSITION = 0x10E;
        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        const int KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll")]
        static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")] //C:\Windows\System32\user32.dll 참조
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        //WM_Keydown
        //https://wiki.winehq.org/List_Of_Windows_Messages
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_IME_STARTCOMPOSITION = 0x010D;

        public enum VKeys : int {
            VK_LBUTTON = 0x01,  //Left mouse button
            VK_RBUTTON = 0x02,  //Right mouse button
            VK_CANCEL = 0x03,  //Control-break processing
            VK_MBUTTON = 0x04,  //Middle mouse button (three-button mouse)
            VK_BACK = 0x08,  //BACKSPACE 
            VK_TAB = 0x09,  //TAB 
            VK_CLEAR = 0x0C,  //CLEAR 
            VK_RETURN = 0x0D,  //ENTER 
            VK_SHIFT = 0x10,  //SHIFT 
            VK_CONTROL = 0x11,  //CTRL 
            VK_MENU = 0x12,  //ALT 
            VK_PAUSE = 0x13,  //PAUSE 
            VK_CAPITAL = 0x14,  //CAPS LOCK 
            VK_HANGUL = 0x15,  //Hangul 
            VK_ESCAPE = 0x1B,  //ESC 
            VK_SPACE = 0x20,  //SPACEBAR
            VK_PRIOR = 0x21,  //PAGE UP 
            VK_NEXT = 0x22,  //PAGE DOWN 
            VK_END = 0x23,  //END 
            VK_HOME = 0x24,  //HOME 
            VK_LEFT = 0x25,  //LEFT ARROW 
            VK_UP = 0x26,  //UP ARROW 
            VK_RIGHT = 0x27,  //RIGHT ARROW 
            VK_DOWN = 0x28,  //DOWN ARROW 
            VK_SELECT = 0x29,  //SELECT 
            VK_PRINT = 0x2A,  //PRINT 
            VK_EXECUTE = 0x2B,  //EXECUTE 
            VK_SNAPSHOT = 0x2C,  //PRINT SCREEN 
            VK_INSERT = 0x2D,  //INS 
            VK_DELETE = 0x2E,  //DEL 
            VK_HELP = 0x2F,  //HELP 
            VK_0 = 0x30,  //0 
            VK_1 = 0x31,  //1 
            VK_2 = 0x32,  //2 
            VK_3 = 0x33,  //3 
            VK_4 = 0x34,  //4
            VK_5 = 0x35,  //5
            VK_6 = 0x36,  //6
            VK_7 = 0x37,  //7
            VK_8 = 0x38,  //8
            VK_9 = 0x39,  //9
            VK_A = 0x41,  //A
            VK_B = 0x42,  //B
            VK_C = 0x43,  //C
            VK_D = 0x44,  //D
            VK_E = 0x45,  //E
            VK_F = 0x46,  //F
            VK_G = 0x47,  //G
            VK_H = 0x48,  //H
            VK_I = 0x49,  //I
            VK_J = 0x4A,  //J
            VK_K = 0x4B,  //K 
            VK_L = 0x4C,  //L
            VK_M = 0x4D,  //M
            VK_N = 0x4E,  //N
            VK_O = 0x4F,  //O
            VK_P = 0x50,  //P
            VK_Q = 0x51,  //Q
            VK_R = 0x52,  //R
            VK_S = 0x53,  //S
            VK_T = 0x54,  //T
            VK_U = 0x55,  //U
            VK_V = 0x56,  //V
            VK_W = 0x57,  //W
            VK_X = 0x58,  //X
            VK_Y = 0x59,  //Y
            VK_Z = 0x5A,  //Z
            VK_NUMPAD0 = 0x60,  //Numeric keypad 0
            VK_NUMPAD1 = 0x61,  //Numeric keypad 1
            VK_NUMPAD2 = 0x62,  //Numeric keypad 2
            VK_NUMPAD3 = 0x63,  //Numeric keypad 3
            VK_NUMPAD4 = 0x64,  //Numeric keypad 4
            VK_NUMPAD5 = 0x65,  //Numeric keypad 5
            VK_NUMPAD6 = 0x66,  //Numeric keypad 6
            VK_NUMPAD7 = 0x67,  //Numeric keypad 7
            VK_NUMPAD8 = 0x68,  //Numeric keypad 8
            VK_NUMPAD9 = 0x69,  //Numeric keypad 9 
            VK_SEPARATOR = 0x6C,  //Separator
            VK_SUBTRACT = 0x6D,  //Subtract
            VK_DECIMAL = 0x6E,  //Decimal
            VK_DIVIDE = 0x6F,  //Divide
            VK_F1 = 0x70,  //F1
            VK_F2 = 0x71,  //F2
            VK_F3 = 0x72,  //F3
            VK_F4 = 0x73,  //F4 
            VK_F5 = 0x74,  //F5 
            VK_F6 = 0x75,  //F6 
            VK_F7 = 0x76,  //F7 
            VK_F8 = 0x77,  //F8 
            VK_F9 = 0x78,  //F9 
            VK_F10 = 0x79,  //F10 
            VK_F11 = 0x7A,  //F11 
            VK_F12 = 0x7B,  //F12 
            VK_SCROLL = 0x91,  //SCROLL LOCK 
            VK_LSHIFT = 0xA0,  //Left SHIFT 
            VK_RSHIFT = 0xA1,  //Right SHIFT 
            VK_LCONTROL = 0xA2,  //Left CONTROL 
            VK_RCONTROL = 0xA3,  //Right CONTROL 
            VK_LMENU = 0xA4,   //Left MENU 
            VK_RMENU = 0xA5,  //Right MENU 
            VK_PLAY = 0xFA,  //Play 
            VK_ZOOM = 0xFB, //Zoom 
            VK_LEFT_SQUARE_BRACKET = 219,  //[
            VK_RIGHT_SQUARE_BRACKET = 221  //]
        }

        static TextBox mainTextbox;

        public MainWindow() {
            InitializeComponent();
            mainTextbox = MainTextBox;
            SetHook();
        }

        private LowLevelKeyboardProc _proc = hookProc;
        private static IntPtr hhook = IntPtr.Zero;

        public void SetHook() {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        public void UnHook() {
            UnhookWindowsHookEx(hhook);
        }

        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam) {
            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN) {
                VKeys vkCode = (VKeys)Marshal.ReadInt32(lParam);
                mainTextbox.AppendText(vkCode.ToString());
                mainTextbox.AppendText(" | ");
            
                //후킹한 키보드 강제 입력
                //keybd_event((byte)Marshal.ReadByte(lParam), 0, KEYEVENTF_EXTENDEDKEY, 0);

                //키 입력을 무효화 한다.
                return (IntPtr)1;
            } else
                //키 입력을 정상적으로 작동하게 한다.
                return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            UnHook();
        }
    }
}