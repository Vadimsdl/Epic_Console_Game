using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static int count = 0; //was the first move made?
        static Game game; //create a variable game
        static string strStatistic = ""; //contains information about the game
        static void Main(string[] args)
        {
            game = new Game(); //initialize the variable
            /*
             * if there have been no entries yet and the bot comes first, 
             * then we assign count 1 and go to the logicbot method in order 
             * to make the first move for the bot, if the player goes first, we skip this part
            */
            if (count == 0 && Game.Move == 0)
            {
                LogicBot();
                count = 1;
            }

            /*
             * here we create an array of method
             * elements that are then called when we select menu items
             */
            var elems = new[] { new Element("Attak"){ Command = Attack },
            new Element("Healt up"){ Command = HPUp },
            new Element("Clear Console"){ Command = Clear },
            new Element("Exit"){ Command = ExitCommandHandler } };

            Menu menu = new Menu(elems);
            while (true)
            {
                menu.Draw(); //draw menu items

                /*
                 * if we pressed any button on the keyboard, 
                 * we start looking in the case for what we pressed, 
                 * if we don't find, we just clear the console
                 */
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.SelectPrev();
                        Clear();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.SelectNext();
                        Clear();
                        break;
                    case ConsoleKey.Enter:
                        menu.ExecuteSelected();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
            
        }

        /*
         * if in the menu item we chose to cure our health, 
         * then we heal ourselves and immediately start the bot's turn, 
         * just as if this is the first move we change count on the fly and 
         * change the move from 0 to 1 and to turn
         */
        private static void HPUp()
        {
            Clear();
            strStatistic = game.HPUp();
            Console.WriteLine(strStatistic);
            LogicBot();
        }

        /*
         * everything is the same as with health
         */
        private static void Attack()
        {
            Clear();
            strStatistic = game.Attack();
            Console.WriteLine(strStatistic);
            LogicBot();
        }

        private static void LogicBot()
        {
            if (strStatistic != "\nBot Win!\n" && strStatistic != "\nPlayer Win!\n") //if there is a winner, the game stops
            {
                Game.Move = 0; //change of turn to bot

                int precent = (Game.HealthBot * 100) / game.MaxHealth; //converting health into percent
                if (precent <= 35) //if the bot has less than 35 percent health, then we increase its chance that it will heal itself
                {
                    int ran = new Random().Next(0, 5);
                    if (ran <= 3)
                    {
                        Console.WriteLine(game.HPUp());
                    }
                    else
                    {
                        Console.WriteLine(game.Attack());
                    }
                }
                else
                {
                    //if more than 35% then we make the same chance to hit and restore health
                    int ran = new Random().Next(0, 2);
                    //if health is at least 70%, it is not healed
                    if (ran == 0 && precent < 70)
                    {
                        Console.WriteLine(game.HPUp());
                    }
                    else
                    {
                        Console.WriteLine(game.Attack());
                    }
                }
            }
        }

        /*
         * clear the console and change the course
         */
        private static void Clear()
        {
            Game.Move = 1;
            count = 1;
            Console.Clear();
        }

        /*
         * exit game 
         */
        private static void ExitCommandHandler()
        {
            Environment.Exit(0);
        }
    }

    delegate void CommandHandler();
    class Menu
    {
        public Element[] Elements { get; set; }
        public int Index { get; set; }

        /*
         * in the constructor we accept an array of elements
         */
        public Menu(Element[] elems) 
        {
            this.Index = 0;
            this.Elements = elems;
            Elements[Index].IsSelected = true;
        }

        /*
         * display menu items
         */
        public void Draw()
        {
            foreach (var element in Elements)
            {
                element.Print();
            }
        }

        /*
         * then by array elements
         * visually by menu item
         * to the next
         */
        public void SelectNext()
        {
            if (Index == Elements.Length - 1) return;
            Elements[Index].IsSelected = false;
            Elements[++Index].IsSelected = true;
        }

        /*
         * then by array elements
         * visually by menu item
         * to the prev
         */
        public void SelectPrev()
        {
            if (Index == 0) return;
            Elements[Index].IsSelected = false;
            Elements[--Index].IsSelected = true;
        }

        /*
         * run the method you chose
         */
        public void ExecuteSelected()
        {
            Elements[Index].Execute();
        }
    }

    class Element
    {
        public string Text { get; set; }
        public ConsoleColor SelectedForeColor { get; set; }
        public ConsoleColor SelectedBackColor { get; set; }
        public bool IsSelected { get; set; }
        public CommandHandler Command;

        /*
         * we pass the text and paint the element
         * that we have active (on which we are)
         */
        public Element(string text) 
        {
            this.Text = text;
            this.SelectedForeColor = ConsoleColor.Black;
            this.SelectedBackColor = ConsoleColor.Gray;
            this.IsSelected = false;
        }

        /*
         * displaying menu items on the console
         */
        public void Print()
        {
            if (this.IsSelected)
            {
                Console.BackgroundColor = this.SelectedBackColor;
                Console.ForegroundColor = this.SelectedForeColor;
            }
            Console.WriteLine(this.Text);
            Console.ResetColor();
        }

        public void Execute()
        {
            if (Command == null) return;
            Command.Invoke();
        }
    }
}
