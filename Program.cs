///////////////////////////////
///Simple memory game in C#
///Author: Michał Kaszuba
///Email: mkaszuba09@gmail.com
///////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace MemoryGame
{
    public class Word       //Single word class containing its name, coordinates and status
    {
        string name;
        int x, y;
        bool isHidden;

        public Word(string namez, int xz, int yz)
        {
            x = xz;
            y = yz;
            name = namez;
            isHidden = true;
        }
        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }
        public string GetName()
        {
            return name;
        }
        public bool GetStatus()
        {
            return isHidden;
        }
        public void SetStatus(bool status)
        {
            isHidden = status;
        }
    }

    public class Board      //Board Class contains score, guess chances, whole board and it manages the whole game.
    {
        public string[,] board; //Board matrix

        List<Word> words = new List<Word>(); //List words contains objects of class Word
        int score, chances;

        public Board()
        {
            board = new string[2, 2];
            score = 0;
            chances = 0;
        }

        public Board(bool mode)
        {
            if (mode)
            {
                board = new string[3, 5] { { "  ", " 1 ", " 2 ", " 3 ", " 4 " }, { " A ", "", "", "", "" }, { " B ", "", "", "", "" } };
                chances = 10;
            }
            else
            {
                board = new string[5, 5] { { "  ", " 1 ", " 2 ", " 3 ", " 4 " }, { " A ", "", "", "", "" }, { " B ", "", "", "", "" }, { " C ", "", "", "", "" }, { " D ", "", "", "", "" } };
                chances = 15;
            }
            score = 0;
        }


        Random random = new Random();

        public void GenB(int guess) //Filling board with random words from file Words.txt \project_directory\bin\Debug\Words.txt
        {
            var rowCount = board.GetLength(0);
            var colCount = board.GetLength(1);
            int x, y, z;

            var WordFile = File.ReadAllLines("Words.txt");


            var wordlist = new List<string>(WordFile);
            List<string> pom = new List<string>();

            for(int i= 0; i<guess; i++)
            {
                z = random.Next(0, wordlist.Count);
                pom.Add(wordlist[z]);
                wordlist.RemoveAt(z);       //words cannot be repeated
            }


            for (int c = 0; c < 2; c++)
            {
                for (int i = 0; i < guess; i++)
                {
                    while (true)
                    {
                        x = random.Next(1, rowCount);
                        y = random.Next(1, colCount);
                        if (board[x, y] != "")
                            continue;
                        else
                        {
                            words.Add(new Word(pom[i], x, y));  //Filling board and list with words
                            board[x, y] = pom[i];
                            break;
                        }
                    }

                }
            }
        }

        public bool CheckS(int xz, int yz)      //Find word status by its coordinates
        {
            foreach (var item in words)
            {
                if (item.GetX() == xz && item.GetY() == yz)
                    return item.GetStatus();
            }
            return false;
        }

        public void SetS(int xz, int yz, bool status)   //Set word status by its coordinates
        {
            foreach (var item in words)
            {
                if (item.GetX() == xz && item.GetY() == yz)
                    item.SetStatus(status);
            }

        }


        public void PrintB(bool hide)               //display board
        {

            var rowCount = board.GetLength(0);
            var colCount = board.GetLength(1);

            Console.WriteLine("-------------------------");
            Console.WriteLine("Level: " + (rowCount < 4 ? "easy" : "hard"));
            Console.WriteLine($"Guess chances: {chances}\n");

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {

                    if (hide)       
                        Console.Write(String.Format("{0}\t", CheckS(row, col) == false ? board[row, col] : "    "));    //Hiding words. If word is hidden, replace it with "    "
                    else           
                        Console.Write(String.Format("{0}\t", board[row, col]));             //Show all words


                }
                Console.WriteLine();
            }
            Console.WriteLine("\n-------------------------");
        }

        public int Turn()               
        {
            int row1, col1, row2, col2;
            string din;
            PrintB(true);
            Console.WriteLine("\n\nEnter first coordinates [rowcol] eg. A2: ");     //get coordinates from user and check if they are correct
            while (true)
            {

                din = Console.ReadLine();

                if (din.Length < 2 || !int.TryParse(din[1].ToString(), out col1))
                {
                    Console.WriteLine("\nWrong coordinates. Try again: ");
                    continue;
                }

                row1 = -1;

                switch (din[0].ToString())        
                {

                    case "A":
                        row1 = 1;
                        break;
                    case "B":
                        row1 = 2;
                        break;
                    case "C":
                        row1 = 3;
                        break;
                    case "D":
                        row1 = 4;
                        break;
                    default:
                        break;
                }

                if (col1 > 0 && col1 < board.GetLength(1) && row1 > 0 && row1 < board.GetLength(0))
                {
                    if (CheckS(row1, col1))             //if coordinates are correct, unhide word
                    {
                        SetS(row1, col1, false);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("\nWrong coordinates. Try again: ");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("\nWrong coordinates. Try again: ");
                    continue;
                }

            }

            PrintB(true);

            Console.WriteLine("\n\nEnter second coordinates [rowcol] eg. A2: ");        //get coordinates from user and check if they are correct
            while (true)
            {
                din = Console.ReadLine();

                if (din.Length < 2 || !int.TryParse(din[1].ToString(), out col2))
                {
                    Console.WriteLine("\nWrong coordinates. Try again: ");
                    continue;
                }

                row2 = -1;

                switch (din[0].ToString())
                {

                    case "A":
                        row2 = 1;
                        break;
                    case "B":
                        row2 = 2;
                        break;
                    case "C":
                        row2 = 3;
                        break;
                    case "D":
                        row2 = 4;
                        break;
                    default:
                        break;
                }

                if (col2 > 0 && col2 < board.GetLength(1) && row2 > 0 && row2 < board.GetLength(0))
                {
                    if (row2 == row1 && col2 == col1)           //user cannot provide identical coordinates
                    {
                        Console.WriteLine("\nYou entered identical coordinates. Try again: ");
                        continue;
                    }
                    else
                    {

                        if (CheckS(row2, col2))                 //if coordinates are correct, unhide word
                        {
                            SetS(row2, col2, false);            
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong coordinates. Try again: ");
                            continue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nWrong coordinates. Try again: ");
                    continue;
                }
            }

            foreach (var item in words)                     //check if the selected words match
            {
                if (item.GetX() == row1 && item.GetY() == col1)
                {
                    foreach (var item2 in words)
                    {
                        if (item2.GetX() == row2 && item2.GetY() == col2)
                        {
                            if (item.GetName() == item2.GetName())
                            {
                                score++;
                                PrintB(true);
                                return 1;
                            }
                        }
                    }
                }
            }

            PrintB(true);
            SetS(row1, col1, true);     //if the selected words don't match, hide them
            SetS(row2, col2, true);


            return 1;
        }

        public int GetScore()
        {
            return score;
        }
        public int GetChances()
        {
            return chances;
        }
        public void DecChances(int c)
        {
            chances -= c;
        }


    }

    class Program

    {
        static void Main(string[] args)
        {

            bool GameOn = true;

            while (GameOn)
            {
                Console.WriteLine("Memory Game in C#\r");
                Console.WriteLine("------------------------\nChoose difficulty: \n 'e' - easy\n 'h' - hard\n");

                int guess = 0;
                Board board = new Board();

                switch (Console.ReadLine())     //choosing difficulty
                {
                    case "e":
                        guess = 4;
                        board = new Board(true);

                        break;
                    case "h":
                        guess = 8;
                        board = new Board(false);
                        break;

                    default:
                        Console.WriteLine("\nWrong parameter. Try again: ");
                        continue;
                }

                board.GenB(guess);              //generating board

                Stopwatch counter = new Stopwatch();
                counter.Start();                //counting time
                while (board.GetScore() < guess && board.GetChances() != 0)
                {
                    Console.Clear();
                    board.DecChances(board.Turn());
                    Thread.Sleep(1500);
                }
                counter.Stop();

                board.PrintB(true);

                if (board.GetScore() == guess)
                {
                    Console.WriteLine("\n\nYou have won the game! Congratulations! ");
                    TimeSpan ts = counter.Elapsed;
                    int temp = (guess == 4 ? 10 - board.GetChances() : 15 - board.GetChances());
                    Console.WriteLine($"You solved the memory game after {temp} turns. It took you {ts.Seconds} seconds.");

                }             
                else
                {
                    Console.WriteLine("\n\nYou have lost :(");
                    board.PrintB(false);
                }




                Console.Write("\nType 'n' to close the app, or type any other key to try again: ");
                if (Console.ReadLine() == "n") GameOn = false;


            }

            return;
        }
    }
}
