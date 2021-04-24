using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Game
    {
        private int maxDamage = 35, minDamage = 10; //the maximum amount of damage that can be dealt and the minimum
        private int maxHeals = 25, minHeals = 17; //the maximum amount of heal that can be dealt and the minimum
        private int choseDamage; //chance of minimum damage or maximum (сrit chance)

        public static int Move { get; set; } = -1; //determine whose move ( 0 - Bot(computer) ; 1 - Player)
        private int Damage { get; set; } //the amount of damage done
        private static int HealthPlayer { get; set; } = 100; //player health
        public static int HealthBot { get; set; } = 100; //bot(computer) health
        public int MaxHealth { get; set; } = 100; //maximum health that can be


        public Game()
        {
            StartGame();
        }

        private void StartGame()
        {
            Move = new Random().Next(0, 2); //if we have just started the game we randomly choose who walks
        }

        public string Attack()
        {
            choseDamage = new Random().Next(0, 100) + 1; //If we get the number 30 or less than 30 then we can do more than moderate damage or, on the contrary, less
            if (HealthBot > 0 && HealthPlayer > 0) //we check whether there was a victory or not if it was not yet then go to if, if it was, we deduce the winner
            {
                if (choseDamage < 31)//If we get the number 30 or less than 30 then we can do more than moderate damage or, on the contrary, less
                {
                    Damage = new Random().Next(minDamage, maxDamage);
                    if (Damage < 25 && Damage > 17)
                    {
                        Attack(); //if we have damage in the range of 24 - 18 then we do a recursion and repeat all the actions again
                    }
                }
                else
                {
                    Damage = new Random().Next(17, 25) + 1; //do moderate damage
                }

                if (Move == 0) //check whose move and subtract damage to the opposite player
                    HealthPlayer = HealthPlayer - Damage;
                else
                    HealthBot = HealthBot - Damage;

                if (HealthBot <= 0)//if, in the course of inflicting damage, we find out that someone has won, we display a line with the winner
                {
                    return "\nPlayer Win!\n";
                }
                else if (HealthPlayer <= 0)
                {
                    HealthPlayer = 0;
                    return "=============Damage=============\n\nGive Damage: " + ((Move == 0) ? "Bots" : "Player") + "\nDamage: " + Damage +
                         "\n\n=============Health=============\n\nBot have healts: " + HealthBot + "\n\nPlayer have healts: " + HealthPlayer +
                         "\n\n==============End===============\n" + "\nBot Win!\n";
                }
                //if no one has won yet, simply display information about the state of the game
                return "=============Damage=============\n\nGive Damage: " + ((Move == 0) ? "Bots" : "Player") + "\nDamage: " + Damage +
                        "\n\n=============Health=============\n\nBot have healts: " + HealthBot + "\n\nPlayer have healts: " + HealthPlayer +
                        "\n\n==============End===============\n";
            } 
            else
            {
                if (HealthBot <= 0)
                    return "\nPlayer Win!\n";
                else
                    return "\nBot Win!\n";
            }
        }

        public string HPUp()
        {
            
            int HPUP = 0; //a variable to display how much we have increased the health of the players
            HPUP = new Random().Next(minHeals, maxHeals) + 1;

            if (HealthBot > 0 && HealthPlayer > 0) //we check whether there was a victory or not if it was not yet then go to if, if it was, we deduce the winner
            {
                if (Move == 0)
                {
                    HealthBot += HPUP;

                    if (HealthBot >= 100) //if we restore our health with 100% we assign 100 and write a message
                    {
                        HealthBot = 100;

                        return "Bot have max health\n\n==============End===============\n";
                    }
                }
                else
                {
                    HealthPlayer += HPUP;
                    if (HealthPlayer >= 100)
                    {
                        HealthPlayer = 100;
                        return "Player have max health\n\n==============End===============\n";
                    }
                }
                return "Heals Up: +" + HPUP + ((Move == 0) ? "\nHealth Bot: " + HealthBot : "\nHealth Player: " + HealthPlayer) + "\n\n==============End===============\n";
            }
            else
            {
                if (HealthBot <= 0)
                    return "\nPlayer Win!\n";
                else
                    return "\nBot Win!\n";
            }
        }


    }
}
