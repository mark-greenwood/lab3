using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace Lab3Q1
{
    public class HelperFunctions
    {
        /**
        * Counts number of words, separated by spaces, in a line.
        * @param line string in which to count words
        * @param start_idx starting index to search for words
        * @return number of words in the line
        */
        public static int WordCount(ref string line, int start_idx)
        {
            // YOUR IMPLEMENTATION HERE
            int count = 0;
            bool characterIsSpace = false;
            bool characterIsLetter = false;
            bool previousCharacterIsSpace = false;
            bool previousCharacterIsLetter = false;



            for (int i = 0; i < (line.Length - start_idx); i++)
            {
                /* Console.WriteLine("Previous CharacterIsLetter Status {0}", previousCharacterIsLetter);
                 Console.WriteLine("Previous CharacterIsSpace Status {0}", previousCharacterIsSpace);
                 Console.WriteLine("CharacterIsLetter Status {0}", characterIsLetter);
                 Console.WriteLine("CharacterIsSpace Status {0}", characterIsSpace);
                 Console.WriteLine(line[i+start_idx]);
                 Console.WriteLine("Count = {0}", count);
                 Console.WriteLine();
                */
                previousCharacterIsSpace = characterIsSpace;
                previousCharacterIsLetter = characterIsLetter;
                characterIsSpace = false;
                characterIsLetter = false;

                if (char.IsWhiteSpace(line[i + start_idx]))
                    characterIsSpace = true;
                else
                    characterIsLetter = true;

                if (i == 0 && characterIsLetter == true)
                {
                    //  Console.WriteLine("First If True");
                    count++;
                }
                else
                {
                    if ((previousCharacterIsSpace == true) && (characterIsLetter == true))
                    {
                        // Console.WriteLine("Second If True");
                        count++;
                    }
                }
            }


            return count;


        }



        /**
        * Reads a file to count the number of words each actor speaks.
        *
        * @param filename file to open
        * @param mutex mutex for protected access to the shared wcounts map
        * @param wcounts a shared map from character -> word count
        */
        public static void CountCharacterWords(string filename,
                                 Mutex mutex,
                                 Dictionary<string, int> wcounts)
        {
           
            //===============================================
            //  IMPLEMENT THIS METHOD INCLUDING THREAD SAFETY
            //===============================================

            string line;  // for storing each line read from the file
            string speaker = "";  // empty character to start
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            int index = 0;
            int numWords = 0;
           
        
           
            while ((line = file.ReadLine()) != null)
            {
                //=================================================
                // YOUR JOB TO ADD WORD COUNT INFORMATION TO MAP
                //=================================================

                index = IsDialogueLine(line, ref speaker);
                // Console.WriteLine(IsDialogueLine(line, ref character)); // Is the line a dialogueLine?
                if (index > 0 && speaker != null)
                {
                    numWords = WordCount(ref line, index);
                    mutex.WaitOne();
                    if (wcounts.ContainsKey(speaker))
                        wcounts[speaker] = wcounts[speaker] + numWords;
                    else
                        wcounts.Add(speaker, numWords);
                    mutex.ReleaseMutex();
                }
               


               

            }

            file.Close(); // Close the file


        }



        /**
         * Checks if the line specifies a character's dialogue, returning
         * the index of the start of the dialogue.  If the
         * line specifies a new character is speaking, then extracts the
         * character's name.
         *
         * Assumptions: (doesn't have to be perfect)
         *     Line that starts with exactly two spaces has
         *       CHARACTER. <dialogue>
         *     Line that starts with exactly four spaces
         *       continues the dialogue of previous character
         *
         * @param line line to check
         * @param character extracted character name if new character,
         *        otherwise leaves character unmodified
         * @return index of start of dialogue if a dialogue line,
         *      -1 if not a dialogue line
         */
        static int IsDialogueLine(string line, ref string character)
        {

            // new character
            if (line.Length >= 3 && line[0] == ' '
                && line[1] == ' ' && line[2] != ' ')
            {
                // extract character name

                int start_idx = 2;
                int end_idx = 3;
                while (end_idx <= line.Length && line[end_idx - 1] != '.')
                {
                    ++end_idx;
                }

                // no name found
                if (end_idx >= line.Length)
                {
                    return 0;
                }

                // extract character's name
                character = line.Substring(start_idx, end_idx - start_idx - 1);
                return end_idx;
            }

            // previous character
            if (line.Length >= 5 && line[0] == ' '
                && line[1] == ' ' && line[2] == ' '
                && line[3] == ' ' && line[4] != ' ')
            {
                // continuation
                return 4;
            }

            return 0;
        }


        /*
         * Sorts characters in descending order by word count
         *
         * @param wcounts a map of character -> word count
         * @return sorted vector of {character, word count} pairs
         */
        public static List<Tuple<int, string>> SortCharactersByWordcount(Dictionary<string, int> wordcount)
        {

            // Implement sorting by word count here
            List<Tuple<int, string>> sortedByValueList = new List<Tuple<int, string>>();

            foreach (KeyValuePair<string, int> character in wordcount.OrderByDescending(key => key.Value))
            {
                sortedByValueList.Add(new Tuple<int, string>(character.Value, character.Key));
            }

            return sortedByValueList;

        }


        /**
         * Prints the List of Tuple<int, string>
         *
         * @param sortedList
         * @return Nothing
         */
        public static void PrintListofTuples(List<Tuple<int, string>> sortedList)
         {

           foreach (Tuple<int, string> character in sortedList)
            {
                Console.WriteLine("Speaker: {0} has {1} words", character.Item2, character.Item1);
            }    

         }
    }
}

