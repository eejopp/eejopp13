
using System.Timers;

System.Timers.Timer timer=new System.Timers.Timer();

bool quit=false;
int vx;
int vy;
int headX;
int headY;
int[,] GameField;
int w = 80, h = 20;
int score=0;
int lifes=3;
int apples;
int level = 0;
bool nextLevel = false;

//Заставка
void SplashScreen()
{

    string[] ss = new string[9];
    ss[0] = "  sss   s    s   sss s   s   ssssss";
    ss[1] = " s   s  s    s  s  s s  s    s     ";
    ss[2] = "  s     ss   s s   s s s     s     ";
    ss[3] = "   s    s s  s sssss ss      ssssss";
    ss[4] = "    s   s  s s s   s s s     s     ";
    ss[5] = "     s  s   ss s   s s  s    s     ";
    ss[6] = "s    s  s    s s   s s   s   s     ";
    ss[7] = "  sss   s    s s   s s    s  ssssss";
    ss[8] = "                                   ";
  

    Console.ForegroundColor = ConsoleColor.Green;
    for(int i=0;i<ss.Length;i++)
        for(int j=0;j<ss[i].Length;j++)
        {
            Console.SetCursorPosition(j+25,i+10 );
            Console.Write(ss[i][j]);
            //System.Console.Beep(200, 1);
            //System.Threading.Thread.Sleep(5);
        }
    Console.SetCursorPosition(30, 25);
    Console.Write("Press any key to start");
    Console.ResetColor();

}

void Init()
{
    Console.CursorVisible = false;
    Console.SetWindowSize(w + 3, h + 6);
    Console.SetBufferSize(w + 3, h + 6);
}

void Load(int level=1)
{
    vx = 0;
    vy = 1;
    headX = w/2;
    headY = h/2;
    GameField = new int[w+1, h+1];
    GameField[headX, headY] = 1;
    Random random = new Random();
    apples = level+1;
    nextLevel = false;
    //разбрасываем яблоки
    for(int i=0;i<apples;i++)
        GameField[random.Next(1,w), random.Next(1,h)] =-1 ;

    //создаем бардюры
    for (int i = 0; i <= w; i++)
    {
        GameField[i, 0] = 10000;
        GameField[i, h] = 10000;
    }
    for (int i = 0; i < h; i++)
    {
        GameField[0, i] = 10000;
        GameField[w, i] = 10000;
    }
}

void Update()
{
    //GameField[headX, headY] = 0;
    //if (GameField[headX + vx, headY + vy] < 0) Next(headX + vx, headY + vy, 1);
    //else
    {
        headX += vx;
        headY += vy;
        if (Collision()) return;

        if (GameField[headX, headY] < 0)
        {
            score++;
            apples--;
            if (apples==0)
            {
                quit=true;
                nextLevel=true;
                return;
            }
            GameField[headX, headY] = -1;
            Next(headX-vx, headY-vy, 1, 1);
            //прорисовка змейки когда съели яблоко
        }
        else
            Next(headX, headY, 1);
        
    }
}

bool Collision()
{
    if (GameField[headX, headY] > 0) quit = true;
    if (headX < 1 || headX >= w || headY < 1 || headY >= h) quit = true;
    return quit;
}
void Next(int tailX,int tailY, int n,int p=0) 
    //отмечается где прорисовать змейку
{
    
    GameField[tailX, tailY] = n+p;

    if (GameField[tailX + 1, tailY] == n+p) Next(tailX + 1, tailY, n + 1,p);
    else
        if (GameField[tailX - 1, tailY] == n+p) Next(tailX - 1, tailY, n + 1,p);
    else
        if (GameField[tailX, tailY - 1] == n+p) Next(tailX, tailY - 1, n + 1,p);
    else
        if (GameField[tailX, tailY + 1] == n+p) Next(tailX, tailY + 1, n + 1,p);
   else 
        if (p==0) GameField[tailX, tailY] = 0;

}

void PrintGameField()
{
    for (int y = 0; y <= h; y++)
        for (int x = 0; x <= w; x++)
        {
            Console.SetCursorPosition(x, y+1);
            
            switch (GameField[x,y])
            {
                case 0:
                    Console.WriteLine(' ');
                    break;
                case -1:
                    Console.WriteLine('&');
                    break;
                case 1:
                    Console.WriteLine('^');
                    break;             
                default:
                    //Console.WriteLine(GameField[x,y]);
                    Console.WriteLine('█');
                    break;
            }
        }
    Console.SetCursorPosition(10, 0);
    Console.Write($"Level:{level} Score:{score} Lifes:{lifes} Apples:{apples}");
}


void KeyboardUpdate()
{
    if (Console.KeyAvailable)
    {

        ConsoleKey key = Console.ReadKey().Key;
        //System.Diagnostics.Debug.WriteLine(key);
       // System.Diagnostics.Debug.WriteLine("X=" + headX + " Y=" + headY + " VX=" + vx + " VY=" + vy);
       // Console.Title = DateTime.Now.ToLongTimeString();
        switch (key)
        {

            case ConsoleKey.LeftArrow:

                vx = -1;
                vy = 0;
                break;
            case ConsoleKey.RightArrow:

                vx = 1;
                vy = 0;
                break;
            case ConsoleKey.UpArrow:

                vx = 0;
                vy = -1;
                break;
            case ConsoleKey.DownArrow:

                vx = 0;
                vy = 1;
                break;
            case ConsoleKey.Escape:
                timer.Stop();
                quit = true;
                Console.WriteLine("Bye-bye");
                break;

        }


    }

}

Init();
SplashScreen();
Console.ReadKey();
Console.Clear();
Load(++level);
while (lifes > 0)
{
    if (nextLevel == true) Load(++level);
    PrintGameField();
    Console.ReadKey();
    while (!quit)
    {
        KeyboardUpdate();
        Update();
        PrintGameField();
        //System.Threading.Thread.Sleep(5);
    };
       quit = false;
    if (nextLevel == false) 
    { lifes--;
        vx = 0;
        vy = 1; GameField[headX, headY] = 0;
        headX = w / 2;
        headY = h / 2;
        GameField[headX, headY] = 1;
    }
};