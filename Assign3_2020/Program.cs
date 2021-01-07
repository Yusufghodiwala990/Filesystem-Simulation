

/* This program uses a particular tree structure called leftMostChild-rightSibling to stimulate a filesystem of a typical operating
 * system. Each level of the file system is denoted by the leftmostchild of that level. Subdirectories of a directory will be first
 * assigned to a leftMostchild AND then upon adding more subdirectories into the same directory, it will have rightSiblings 
 * accordingly
 * 



*/

/* DONE BY : YUSUF GHODIWALA AND DAUD JUSAB


/*---------------------------------------------------------NOTE--------------------------------------------------------------------
 * 
 * Since there is a bit of leeway as to how EXACTLY an address format should look like for the functions of a filesystem, below
 * is how this program implements addresses for each function;
 * 
 * Adding Directory => For adding directories, simply pass an address that brings the program to the level and do NOT include the
 *                     directory name IN the address. E.g To add directories inside of the root ("/") 
 *                     simply enter "/". The program will first check the validity of the address and once it reaches the 
 *                     particular level, it will then ask for the directory name that you want to add to THAT level.
 *                     Another example; if inside root,we had a 'Math' directory and suppose we want to add a directory 'Math1350'
 *                     INSIDE of 'Math', then we would simply pass an address as such: "/Math". It will check for same names
 *                     at the same level accordingly and deny if a directory with the same name already exists. If there was 
 *                     another directory to be added INSIDE of 'Math' like 'Math1550' then again the address would simply look
 *                     like "/Math". We ONLY need the level and NOT the directory name.
 * 
 * 
 * Removing Directory => Addresses for Removing Directories MUST include the directory name that needs to be deleted. 
 *                      For example, If root had 3 directories "/Math", "/COIS", "/Biology" then to delete "Math",
 *                       We would simply enter "/Math". It will assign leftmostchild of root accordingly and rightsiblings
 *                       will be adjusted.
 *           
 *      
 * Adding File =>  Addresses for Adding file, as with Adding Directories, should just include the path to reach to that 
 *                 directory and NOT  the name of the file.
 *                 E.g, if root had 'Math' directory and we wanted to add file to it,
 *                 we would simply pass the address "/Math". The program will ask for a file name once it reaches to 
 *                 the level based on the address given and then it will be added. 
 *                 
 * 
 * 
 * Removing File => Addresses for Removing Files, as with Removing Directories, MUST include the file that needs to be deleted
 *                  E.g, If Root had a sub directory 'Math' and 'Math' had a file, "Hello", Then address will be "/Math/Hello"
 *                  to be able to remove that file from 'Math'
 *                     
 *                     
 *                     
 *  (This had to be cleared before the program is used to avoid any confusion)
 * 
 * */





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Setting up the node class. Each node will have a reference to it's leftMostChild and RightSibling. Each node will store
//  a directory name and a generic List of files(strings). 
public class Node
{
    public string Directory { get; set; }
    public List<string> File { get; set; }
    public Node LeftMostChild { get; set; }
    public Node RightSibling { get; set; }


    //Constructor
    public Node(string directory, List<string> file, Node leftMostChild, Node rightSibling)
    {
        this.Directory = directory;
        this.File = file;
        this.LeftMostChild = leftMostChild;
        this.RightSibling = rightSibling;


    }

}


// Filesystem class which implements a leftMostChild-Rightsibling tree to store directories and files
public class FileSystem
{
    private Node root; //reference to the root

    int numFiles;      //variable to keep track of how many files are in the filesystem



    // Constructor 
    public FileSystem()
    {
        List<string> file = new List<string>();   // initializing a List of strings and adding it to the root

        root = new Node("/", file, null, null);  // By default, the file system will always have a root ("/")



    }


    //Method : AddFile
    //Parameters: Takes the string address from the user
    //Return Type : bool
    //Description: This method parses the address given by the user first and then traverses the tree based on the address.
    //             If an address is valid, it then asks the user for the name of the file and adds it accordingly and returns true,
    //             false otherwise. Same file name isn't allowed in a single directory and the method will return false.
    //           
    public bool AddFile(string address)
    {

        string[] path = Parsing(address);     // calling the parsing method and storing the array returned in path[]
        Node curr = root;                     // assigning a reference to the root
        bool found = false;                   // initializing a bool flag to false

        if (address == "")                      // if user enters an empty address
        {
            Console.WriteLine("You cannot pass an empty address");
            return false;
        }
        else                                    // else traverse the array containing the address
        {
            // with each iteration, this whole loop will check weather the current
            // string in the array matches the directory name in curr. It checks
            //  all the rightsiblings whenever curr goes down a level(leftmostchild)
            for (int i = 0; i <= path.Count() - 1; i++)                                              
                                                          
                if (path[i] == "")            // this check is based on the root. Since splitting of the address                                                
                {                             // is based on slashes and root= '/' thus the first entry in the address
                    found = true;             // will be empty(if entered correctly)
                }
                else
                {
                    found = false;                                   // found becomes false
                    curr = curr.LeftMostChild;                       // moving down a level
                    if (curr != null && curr.Directory == path[i])  // checking if curr is equal to the path given
                    {

                        found = true;                                 // found becomes true
                    }
                    else                                             // else, go through all the rightsiblings at the current level
                    {

                        // traversing until there are no rightsbilings and we haven't found the directory specified in the address
                        while (curr != null && curr.RightSibling != null && !found)  
                        {
                            curr = curr.RightSibling;

                            if (curr.Directory == path[i])   // if we find the directory given in the address
                            {
                                found = true;                // found becomes true and the while loop will fail


                            }

                        }

                    }

                }



            }

            if (!found)      // if after the entire traversal, found remained false then the path given was wrong
            {
                Console.WriteLine("Sorry the path was wrong");
                return false;

            }
            else           // else curr is exactly at the level/place specified by the address
            {
                string filename;
               

                // asking for a filename
                Console.WriteLine("What is the name of the file you want to add?");
                filename = Console.ReadLine();

                if (filename == "")      // if user enters an empty file name
                {
                    Console.WriteLine("Sorry, the file name cannot be empty");
                    return false;
                }
                else
                {
                    if (curr.File != null && curr.File.Contains(filename))   // if file already exists in the List array
                    {
                        Console.WriteLine("Sorry same file name isn't allowed");
                        return false;
                    }
                    else
                    {
                        curr.File.Add(filename);    // adding the file to the current node's List.
                        Console.WriteLine("File '{0}' added in the directory : {1}", filename, curr.Directory);
                        numFiles++;             // increasing the counter for number of files in the filesystem
                        return true;
                    }
                }


            }

        }





    //Method : RemoveFile
    //Parameters: Takes the string address from the user
    //Return Type : bool
    //Description: This method parses the address given by the user first and then traverses the tree based on the address.
    //             The filename to be deleted shall be included in the address and will be removed if the file is found
    //             in the directory specified in the address           
    public bool RemoveFile(string address)
    {

        string[] path = Parsing(address);             // calling the parsing method and storing the array returned in path[]
        Node curr = root;                             // assigning a reference to the root
        bool found = false;                           // initializing a bool flag to false

        if (address == "")                            // if user enters an empty address
        {
            Console.WriteLine("You cannot pass an empty address");
            return false;
        }
        else                                           // else traverse the array containing the address
        {
            // with each iteration, this whole loop will check weather the current
            // string in the array matches the directory name in curr. It checks
            //  all the rightsiblings whenever curr goes down a level(leftmostchild)

            // iterating only until second last item in the array that has the address since the last item is the file itself
            for (int i = 0; i < path.Count() - 1; i++)
            {

                if (path[i] == "")         // this check is based on the root. Since splitting of the address
                {                          // is based on slashes and root= '/' thus the first entry in the address
                    found = true;          // will be empty(if entered correctly)
                }
                else
                {
                    found = false;                                    // found becomes false
                    curr = curr.LeftMostChild;                        // moving down a level
                    if (curr != null && curr.Directory == path[i])    // checking if curr is equal to the path given
                    {
                        found = true;                                 // found becomes true
                    }
                    else                                         // else, go through all the rightsiblings at the current level
                    {

                        // traversing until there are no rightsbilings and we haven't found the directory specified in the address
                        while (curr != null && curr.RightSibling != null && !found)
                        {
                            curr = curr.RightSibling;

                            if (curr.Directory == path[i])          // if we find the directory given in the address
                            {
                                found = true;                       //found becomes true and the while loop will fail
                            }
                        }
                    }
                }
            }

            if (!found)             // if after the entire traversal, found remained false then the path given was wrong
            {
                Console.WriteLine("Sorry the path was wrong");
                return false;

            }
            else
            {
                if (curr.File.Contains(path[path.Count()-1]))  //Checking if the List array has the file mentioned in the address
                {
                    curr.File.Remove(path[path.Count()-1]);   //removing the file
                    Console.WriteLine("File '{0}' has been Removed from the directory : {1}", path[path.Count()-1], curr.Directory);
                    numFiles--;     // decreasing the numfiles counter
                    return true;
                }
                else          // else if it doesn't exist
                {
                    Console.WriteLine("Sorry there is no such file in : {0}", curr.Directory);
                    return false;
                }

            }


        }
    }

    //Method : AddDirectory
    //Parameters: Takes the string address from the user
    //Return Type : bool
    //Description: This method parses the address given by the user first and then traverses the tree based on the address.
    //             If an address is valid, it then asks the user for the name of the directory and adds it accordingly and 
    //             returns true, false otherwise. The address should include the level that the new directory needs to be
    //              added. Address should NOT include the actual directory name to be added.            
    public bool AddDirectory(string address)
    {
        List<string> File = new List<string>();        //initializing a List of strings
        int i;
        string[] path = Parsing(address);             // calling the parsing method and storing the array returned in path[]
        bool found = false;                           // initializing a bool flag to false
        Node curr = root;                             // assigning a reference to the root
        Node prev = null;                             // prev will be used to keep track of the previous level each time curr moves
                                                      // down.

        if (address == "")               // if user enters an empty address
        {
            Console.WriteLine("You cannot pass an empty address");
            return false;
        }
        else
        {
            // with each iteration, this whole loop will check weather the current
            // string in the array matches the directory name in curr. It checks
            //  all the rightsiblings whenever curr goes down a level(leftmostchild)
            for (i = 0; i <= path.Count() - 1; i++)
            {

                if (path[i] == "")        // this check is based on the root. Since splitting of the address
                {                         // is based on slashes and root= '/' thus the first entry in the address
                    prev = curr;          // will be empty(if entered correctly)
                    found = true;
                }
                else
                {
                    found = false;                                      // found becomes false
                    curr = curr.LeftMostChild;                          // moving down a level

                    // checking if curr is equal to the path given
                    if (curr != null && curr.Directory == path[i])
                    {
                        prev = curr;             //adjusting prev accordingly
                        found = true;
                    }
                    else           
                    {
                        prev = curr;            //adjusting prev accordingly

                        // traversing until there are no rightsbilings and we haven't found the directory specified in the address
                        while (curr != null && curr.RightSibling != null && !found)
                        {
                            curr = curr.RightSibling;

                            if (curr.Directory == path[i])        // if we find the directory given in the address
                            {
                                found = true;                     //found becomes true and the while loop will fail
                                prev = curr;                      // adjusting prev accordingly

                            }

                        }

                    }

                }

            }
            if (!found)
            {
                Console.WriteLine("Sorry the path was wrong");
                return false;
            }
            else
            {
                string name;
                if (prev.LeftMostChild == null)             // if prev's leftmostchild is null then add the directory there
                {
                    Console.WriteLine("Give a name");
                    name = Console.ReadLine();
                    if (name == "")                         // if directory name is empty
                    {
                        Console.WriteLine("Sorry, directory name cannot be empty");
                        return false;
                    }
                    else
                    {
                        prev.LeftMostChild = new Node(name, File, null, null);
                        Console.WriteLine("The directory has been added successfully");
                    }

                }
                else          //else if prev's leftmostchild isn't null, go through the rightsiblings and enter it whenever                   
                {              // an empty spot is found

                    curr = curr.LeftMostChild;             // bring curr down a level
                    Console.WriteLine("Give a name");
                    name = Console.ReadLine();
                    while (curr.RightSibling != null || curr.Directory == name)         // while traversing if a directory name                                                                               
                    {                                                                    // already exists at the same level
                        if (curr.Directory == name)
                        {
                            Console.WriteLine("Sorry the directory already exists");
                            return false;
                        }
                        curr = curr.RightSibling;
                    }
                    if (name == "")
                    {
                        Console.WriteLine("Sorry, directory name cannot be empty");
                        return false;
                    }
                    else         // adding the directory at an empty spot
                    {
                        curr.RightSibling = new Node(name, File, null, null);
                        Console.WriteLine("The directory has been added successfully");
                    }

                }
                return true;
            }
        }
    }

    //Method : RemoveDirectory
    //Parameters: Takes the string address from the user
    //Return Type : bool
    //Description: This method parses the address given by the user first and then traverses the tree based on the address.
    //             The direcotry to be deleted shall be included in the address and will be removed if the directory is found
    //             in the directory specified in the address. References will be adjusted accordingly
    public bool RemoveDirectory(string address)
    {
        List<string> File = new List<string>();              //initializing a List of strings
        int i;
        string[] path = Parsing(address);                   // calling the parsing method and storing the array returned in path[]
        bool found = false;                                 // initializing a bool flag to false
        
        Node curr = root;                                   // assigning a reference to the root
        
        Node prev = root;                                   // prev will stay one node behind curr to adjust references when
                                                            // deletion is done

        Node before = root;                                 // before will be used to stay at the previous level when curr moves down
                                                            //  it will be used when a leftmostchild gets deleted to adjust 
                                                            //   references.

      
        if (address == "")         // if the address is empty
        {
            Console.WriteLine("You cannot pass an empty address");
            return false;
        }
        else if (address == "/")          // if user tries to remove the root :)
        {
            Console.WriteLine("You cannot remove the root");
            return false;
        }
        else
        {
            /// with each iteration, this whole loop will check weather the current
            // string in the array matches the directory name in curr. It checks
            //  all the rightsiblings whenever curr goes down a level(leftmostchild)
            for (i = 0; i <= path.Count() - 1; i++)
            {

                if (path[i] == "")                    // this check is based on the root. Since splitting of the address
                {                                     // is based on slashes and root= '/' thus the first entry in the address
                    found = true;                     // will be empty(if entered correctly)
                }
                else
                {
                    found = false;
                    curr = curr.LeftMostChild;         //curr moves down a level
                    if (curr != null && curr.Directory == path[i])          // if we are at the right level based on the address
                    {
                        if (curr.Directory == path[path.Count() - 1])     // if curr(leftmostchild) is the actual directory 
                        {                                                 // that needs to be deleted
                            before = prev;
                            prev.LeftMostChild = curr.RightSibling;      // prev will now point to curr's rightsibling
                            found = true;

                        }
                        else            // else we bring down prev to curr and iterate again
                        {
                            prev = curr;                
                            found = true;
                        }

                    }
                    else                    // if the directory to be deleted is in a rightsibling
                    {
                        while (curr != null && curr.Directory != path[i])      // looping through all the rightsiblings 
                        {
                            prev = curr;
                            curr = curr.RightSibling;
                        }
                        if (curr != null && curr.Directory == path[i])    // if the address is correct
                        {
                            if (curr.Directory == path[path.Count() - 1])      // if curr is directory to be deleted
                            {
                                prev.RightSibling = prev.RightSibling.RightSibling;    // move around curr using prev
                                found = true;

                            }
                            else            // will be used to iterate again
                            {
                                prev = curr;         
                                before = prev;
                                found = true;
                            }
                        }
                        else       // after reaching the last segment of the address and the path is wrong
                        {
                            Console.WriteLine("The path was not found");
                            found = false;
                        }
                    }

                }

            }
            if (!found)               //if found remained false in the entire iteration
                return false;
            else
                Console.WriteLine("The directory has been removed");
            return true;
        }

    }


    //Method : NumberFiles
    //Parameters: None
    //Return Type : int
    //Description: Returns the number of files in the file system
    
    public int NumberFiles()
    {
        return numFiles;
    }


    //Method : PrintFileSystem
    //Parameters: None
    //Return Type : string
    //Description: Prints the entire file system in a pre order fashion. This public method calls a private method
    public string PrintFileSystem()
    {
        string sysAddress = "";
        sysAddress = PreOrderTrav(root, sysAddress);          //calling the actual recusive function to print the filesystem
        return sysAddress.Remove(0, 1);


    }

    //Method : PreOrderTrav
    //Parameters: curr(reference to the root) and address
    //Return Type : string
    //Description: Prints the entire file system in a pre order fashion recursively. It appends the address in each recursive call
    private string PreOrderTrav(Node curr, string address)
    {
        if (curr == null)                // base case
        {
            // do nothing
        }

        else
        {
            address += curr.Directory + '/';    // appending the current directory name 
            foreach (string s in curr.File)     // appending each file in the current directory 
            {
                address += s + '/';
            }

            address = PreOrderTrav(curr.LeftMostChild, address);           // Recursively traverse all leftmostchildren first
            address = PreOrderTrav(curr.RightSibling, address);            // Recursively traverse all rightsiblings second


        }

        return address;
    }



    //Method : Parsing
    //Parameters: address
    //Return Type : string array[]
    //Description: Splits the address based on each slash and stores the results into an array
    public string[] Parsing(string address)
    {

        string[] f = address.Split('/');          //using String class's split method
        return f;

    }




}

class Program
{
    public static void Main(string[] args)
    {


        FileSystem t = new FileSystem();
        //char command;

        // interface
        Console.WriteLine("\nHere are your options:\n1. Add Directory\n2. Remove Directory\n3. Add File\n4. Remove File\n5. Find Number of Files in Filesystem\n6. Print File System\n7. Exit");
        char choice = Convert.ToChar(Console.ReadLine());

        Console.Clear();     
        while (choice != '7')
        {
            switch (choice)
            {

                case '1':
                    Console.WriteLine("At what address do you want to add a directory?");
                    t.AddDirectory(Console.ReadLine());
                    break;
                case '2':
                    Console.WriteLine("What is the address of the directory you want to delete?");
                    t.RemoveDirectory(Console.ReadLine());
                    break;
                case '3':
                    Console.WriteLine("At what address would you like to add a file?");
                    t.AddFile(Console.ReadLine());
                    break;
                case '4':
                    Console.WriteLine("What is the address of the file you want to delete?");
                    t.RemoveFile(Console.ReadLine());
                    break;
                case '5':
                    Console.WriteLine("Number of files in the entire filesystem:{0}", t.NumberFiles());
                    break;
                case '6':
                    string address;
                    address = t.PrintFileSystem();
                    Console.WriteLine(address);
                    break;
                case '7':
                    return;
                default:
                    Console.WriteLine("Not a valid option");
                    break;
            }
            Console.WriteLine("\nHere are your options:\n1. Add Directory\n2. Remove Directory\n3. Add File\n4. Remove File\n5. Find Number of Files in Filesystem\n6. Print File System\n7. Exit");
            choice = Convert.ToChar(Console.ReadLine());
            Console.Clear();

        }
        Console.WriteLine("See you next time");
        Console.ReadLine();
    }



}
