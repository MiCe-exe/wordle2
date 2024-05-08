using System.Diagnostics;
using System.Threading.Tasks.Sources;

namespace Wordle2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //loop exit
            bool exitGame = false;

            //Word List vars / word picked
            List<string> WordList = new List<string>();
            WordList = GetWordList("wordleList.txt");
            string puzzleWord = "";

            //Random number var
            Random rnd = new Random();

            // User word
            string userInput = "";

            // Keep score
            int score = 0;
            

            //Console.WriteLine("\u001b[38;5;17m\u001b[48;5;217m Hello world \u001b[0m");
            // https://codehs.com/tutorial/ryan/add-color-with-ansi-in-javascript

            while (!exitGame)
            {
                //get a random word and remove from the list
                //Also convert into lower case.
                puzzleWord = WordList[rnd.Next(0, WordList.Count - 1)].ToLower();
                WordList.Remove(puzzleWord);

                //Display the clue how big the word is
                Console.WriteLine("\n~ Current Word ~");
                Console.WriteLine(new string('#', puzzleWord.Length));

                //Cheat so we can see if our code is working when we try to solve
                //Make sure to comment it out when it goes live
                Console.WriteLine("+++++++++ Cheat Enable +++++++++");
                Console.WriteLine(puzzleWord + " " + puzzleWord.Length);
                Console.WriteLine("++++++++++++++++++++++++++++++++");
                // =========================================================

                // Go into a for loop bassed on turns. Since Wordle allows only 5 turns
                // The loop will only last 5 turns.
                for (int i = 0; i < 5; i++)
                {
                    // Display Number of tries left
                    Console.WriteLine("Tries left: " + (5 - i));

                    // Get user input
                    Console.WriteLine("\nGuess your Word: ");
                    userInput = GetInput(puzzleWord.Length);

                    if(CheckWord(puzzleWord, userInput))
                    {
                        Console.WriteLine("Puzzle solved! GG.");
                        score += (5 - i);
                        Console.WriteLine($"Total Score: {score}");
                        break;
                        
                    }
                    else
                    {
                        Console.WriteLine("\nNice Try.");
                    }

                }

                //End of the loop lets ask the user if they want to quit
                Console.Write("Do you wish to exit? Y/N: ");
                if(char.ToLower(Console.ReadKey().KeyChar) == 'y')
                {
                    exitGame = true;
                }
                else
                {
                    Console.Clear();
                }

            }

        }

        static List<string> GetWordList(string txtFile) 
        {
            List<string> words = new List<string>();

            try
            {
                foreach(string line in System.IO.File.ReadAllLines(@txtFile))
                {
                    words.Add(line);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return words;
        }

        static string GetInput(int maxLen)
        {
            string input = "";

            while(input.Length != maxLen)
            {
                // Using a try catch just in case invalid input is used
                try
                {
                    // Convert input to lower case
                    // We do this so case sensitivity wont give a
                    // false postive or a false negative
                    input = Console.ReadLine().ToLower();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                // Lets check if the input length is correct
                // if it is not the correct length throw a warning
                // The while loop will continue also because the length does not match
                if(input.Length != maxLen)
                {
                    Console.WriteLine("Please enter the correct word length of {0}\n", maxLen);
                }
            }

            return input;
        }

        static bool CheckWord(string puzzle, string input)
        {
            // lets check right off the start if both
            // the guessed word and the puzzle word is the same
            if(puzzle == input)
            {
                // ****** Add green background
                Console.WriteLine("\u001b[48;5;46m\u001b[38;5;0m" + input + "\u001b[0m");
                // ****** Reset Console color to grey?
                return true;
            }

            // Lets add a Dictionary this will help us keep track how many letters
            Dictionary<char, int> charCounter = new Dictionary<char, int>();

            //Wrong char
            bool wrongChar = false;

            // here we're going to use a foreach loop.
            // Adding new char to the dictionary
            // incrementing char if it exist
            foreach (char c in puzzle)
            {
                if(charCounter.ContainsKey(c))
                {
                    charCounter[c]++;
                }
                else
                {
                    charCounter.Add(c, 1);
                }
            }

            //Start a loop for the user word 
            for(int i = 0; i < input.Length; i++)
            {
                // Skip to the next iteration if character not found
                if (!charCounter.ContainsKey(input[i]) || charCounter[input[i]] <= 0)
                {
                    Console.Write(input[i]);
                    continue;
                }

                // start a loop fir the puzzle word bot these loops
                // search for each letter.
                // How it works the user guessed word will start at i position
                // The word puzzle will start at j position.
                // j will increment till end of string and then  i will move up 
                // and j will start all over again.
                for (int j = 0; j < puzzle.Length; j++)
                {
                    //Lets check if character is found and also matches the same position
                    if(i == j && puzzle[j] == input[i])
                    {
                        // Going to decrease the value of the 
                        charCounter[puzzle[j]]--;

                        //print the resaults
                        Console.Write("\u001b[48;5;46m\u001b[38;5;0m" + input[i] + "\u001b[0m");

                        break;
                    }

                    // We put our coding faith for the top if statement but We 
                    // must check still if they are equal if so it must be in the wrong position
                    if (puzzle[j] == input[i] && !(puzzle[i] == input[i]))
                    {
/*                        Debug.WriteLine($"{puzzle[j] == input[i]} -- {!(puzzle[j] == input[j])}");
                        Debug.WriteLine($"{puzzle[j]} and {input[i]} -- {puzzle[j]} and {input[j]}");
                        Debug.WriteLine($"I: {i} - J: {j}\n\n");*/

                        charCounter[puzzle[j]]--;

                        Console.Write("\u001b[48;5;220m\u001b[38;5;0m" + input[i] + "\u001b[0m");

                        break;
                    }

                    // We add this so 
                    if(j == puzzle.Length - 1)
                    {
                        wrongChar = true;
                    }
                    
                }

                // if all if statements fail write out wrong char
                if(wrongChar)
                {
                    Console.Write(input[i]);
                    wrongChar = false;
                }
                
            }

            /*foreach (var pair in charCounter)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }*/

            return false;
        }
    }
}

