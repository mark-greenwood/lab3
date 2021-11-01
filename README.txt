Mark Greenwood


•	How can you test your program without needing to manually go through all the dialogue in Shakespeare's plays?
o	I can confirm that the total number of words counted in correct by using google to determine the number of words in the play and then adding up all the speakers to see if the numbers are the same. 
o	Alternatively, I could unit test different lines of the plays to ensure that the numbers line up. Making sure to think about boundary cases.
•	Has writing this code multithreaded helped in any way? Show some numbers in your observations. If your answer is no, under what conditions can multithreading help?
o	Multithread Run Time = 0.55 seconds
o	Single Thread Run Time = 0.17 seconds
o	Multithreading this code increased the execution time. Multithreading would help if we had separate dictionaries for each play and then ensuring that each one had exclusive access to it. This way, no threads would be waiting for access.
o	This could be done by using an array of dictionaries and then passing each dictionary to the counting function. 
•	As written, if a character in one play has the same name as a character in another -- e.g. King -- it will treat them as the same and artificially increase the word count. How can you modify your code to treat them as separate, but still store all characters in the single dictionary (you do not need to implement this... just think about how you would do it)?
o	I would create a dictionary for each play and then pass the dictionary to the function and then write a function to combine it at the end.
o	Alternatively, you could prepend the title of each play to the character names so that each character name would be unique.

