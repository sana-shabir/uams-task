using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAMS
{
    // Student Class
    class Student
    {
        public string name;
        public int age;
        public double fscMarks;
        public double ecatMarks;
        public double merit;
        public List<DegreeProgram> preferences;
        public List<Subject> regSubject;
        public DegreeProgram regDegree;

        public Student(string name, int age, double fscMarks, double ecatMarks, List<DegreeProgram> preferences)
        {
            this.name = name;
            this.age = age;
            this.fscMarks = fscMarks;
            this.ecatMarks = ecatMarks;
            this.preferences = preferences;
            regSubject = new List<Subject>();
        }

        public void calculateMerit()
        {
            // Merit calculation logic
            merit = 0.7 * fscMarks + 0.3 * ecatMarks;
        }

        public int getCreditHours()
        {
            int count = 0;
            for (int i=0;i<regSubject.Count;i++)
            {
                count = count + regSubject[i].creditHours;
            }
            return count;
        }

        public float calculateFee()
        {
            float fee = 0;
            foreach (Subject sub in regSubject)
            {
                fee = fee + sub.subjectFees;
            }
            return fee;
        }

        public void regStudentSubject(Subject s)
        {
            int stCH = getCreditHours();
            if (regDegree != null && regDegree.isSubjectExists(s) && stCH + s.creditHours <= 9)
            {
                regSubject.Add(s);
                Console.WriteLine("Subject Registered Successfully!");
            }
            else
            {
                Console.WriteLine("A student cannot have more than 9 CH or Wrong Subject");
            }
        }
    }
    
    // Subject Class
    class Subject
    {
        public string code;
        public string type;
        public int creditHours;
        public int subjectFees;

        public Subject(string code, string type, int creditHours, int subjectFees)
        {
            this.code = code;
            this.type = type;
            this.creditHours = creditHours;
            this.subjectFees = subjectFees;
        }
    }

    // DegreeProgram Class
    class DegreeProgram
    {
        public string degreeName;
        public float degreeDuration;
        public List<Subject> subjects;
        public int seats;

        public DegreeProgram(string degreeName, float degreeDuration, int seats)
        {
            this.degreeName = degreeName;
            this.degreeDuration = degreeDuration;
            this.seats = seats;
            subjects = new List<Subject>();
        }

        public int calculateCreditHours()
        {
            int count = 0;
            foreach (Subject sub in subjects)
            {
                count = count + sub.creditHours;
            }
            return count;
        }

        public bool isSubjectExists(Subject sub)
        {
            foreach (Subject s in subjects)
            {
                if (s.code == sub.code)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddSubject(Subject s)
        {
            int creditHours = calculateCreditHours();
            if (creditHours + s.creditHours <= 20)
            {
                subjects.Add(s);
                Console.WriteLine("Subject Added Successfully!");
            }
            else
            {
                Console.WriteLine("20 credit hour limit exceeded");
            }
        }
    }

    // Main Program
    class Program
    {
        static List<Student> students = new List<Student>();
        static List<DegreeProgram> programs = new List<DegreeProgram>();

        static void Main(string[] args)
        {
            int option;
            do
            {
                Console.WriteLine("****************************** UAMS ******************************");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Add Degree Program");
                Console.WriteLine("3. Generate Merit");
                Console.WriteLine("4. View Registered Students");
                Console.WriteLine("5. View Students of a Specific Program");
                Console.WriteLine("6. Register Subjects for a Specific Student");
                Console.WriteLine("7. Calculate Fees for all Registered Students");
                Console.WriteLine("8. Exit");
                Console.Write("Enter Option: ");
                option = int.Parse(Console.ReadLine());

                if (option == 1)
                {
                    // Add Student
                    Console.Write("Enter Student Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter Student Age: ");
                    int age = int.Parse(Console.ReadLine());
                    Console.Write("Enter FSC Marks: ");
                    double fsc = double.Parse(Console.ReadLine());
                    Console.Write("Enter ECAT Marks: ");
                    double ecat = double.Parse(Console.ReadLine());

                    Console.WriteLine("Available Degree Programs:");
                    for (int i = 0; i < programs.Count; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + programs[i].degreeName);
                    }

                    Console.Write("Enter how many preferences: ");
                    int prefCount = int.Parse(Console.ReadLine());
                    List<DegreeProgram> preferences = new List<DegreeProgram>();

                    for (int i = 0; i < prefCount; i++)
                    {
                        Console.Write("Enter Preference " + (i + 1) + ": ");
                        int pref = int.Parse(Console.ReadLine());
                        preferences.Add(programs[pref - 1]);
                    }

                    Student s = new Student(name, age, fsc, ecat, preferences);
                    students.Add(s);
                    Console.WriteLine("Student Added Successfully!");
                }
                else if (option == 2)
                {
                    // Add Degree Program
                    Console.Write("Enter Degree Name: ");
                    string degName = Console.ReadLine();
                    Console.Write("Enter Degree Duration: ");
                    float duration = float.Parse(Console.ReadLine());
                    Console.Write("Enter Seats for Degree: ");
                    int seats = int.Parse(Console.ReadLine());

                    DegreeProgram d = new DegreeProgram(degName, duration, seats);

                    Console.Write("Enter How many Subjects to Enter: ");
                    int subCount = int.Parse(Console.ReadLine());

                    for (int i = 0; i < subCount; i++)
                    {
                        Console.WriteLine("Enter Subject " + (i + 1) + " Details:");
                        Console.Write("Enter Subject Code: ");
                        string code = Console.ReadLine();
                        Console.Write("Enter Subject Type: ");
                        string type = Console.ReadLine();
                        Console.Write("Enter Subject Credit Hours: ");
                        int ch = int.Parse(Console.ReadLine());
                        Console.Write("Enter Subject Fees: ");
                        int fees = int.Parse(Console.ReadLine());

                        Subject sub = new Subject(code, type, ch, fees);
                        d.AddSubject(sub);
                    }

                    programs.Add(d);
                    Console.WriteLine("Degree Program Added Successfully!");
                }
                else if (option == 3)
                {
                    // Generate Merit
                    Console.WriteLine("Generating Merit...");
                    foreach (Student stu in students)
                    {
                        stu.calculateMerit();

                        // Simple merit logic - assign to first preference with available seats
                        foreach (DegreeProgram prog in stu.preferences)
                        {
                            if (prog.seats > 0)
                            {
                                stu.regDegree = prog;
                                prog.seats--;
                                Console.WriteLine(stu.name + " allocated to " + prog.degreeName);
                                break;
                            }
                        }
                    }
                    Console.WriteLine("Merit Generation Complete!");
                }
                else if (option == 4)
                {
                    // View Registered Students
                    Console.WriteLine("\nRegistered Students:");
                    foreach (Student stu in students)
                    {
                        if (stu.regDegree != null)
                        {
                            Console.WriteLine("Name: " + stu.name + ", Degree: " + stu.regDegree.degreeName);
                        }
                    }
                }
                else if (option == 5)
                {
                    // View Students of a Specific Program
                    Console.Write("Enter Program Name: ");
                    string progName = Console.ReadLine();

                    Console.WriteLine("\nStudents in " + progName + ":");
                    foreach (Student stu in students)
                    {
                        if (stu.regDegree != null && stu.regDegree.degreeName == progName)
                        {
                            Console.WriteLine("Name: " + stu.name);
                        }
                    }
                }
                else if (option == 6)
                {
                    // Register Subjects for a Specific Student
                    Console.Write("Enter Student Name: ");
                    string stuName = Console.ReadLine();

                    Student foundStudent = null;
                    foreach (Student stu in students)
                    {
                        if (stu.name == stuName)
                        {
                            foundStudent = stu;
                            break;
                        }
                    }

                    if (foundStudent != null && foundStudent.regDegree != null)
                    {
                        Console.WriteLine("\nAvailable Subjects for " + foundStudent.regDegree.degreeName + ":");
                        foreach (Subject sub in foundStudent.regDegree.subjects)
                        {
                            Console.WriteLine(sub.code + " - " + sub.type + " (" + sub.creditHours + " CH) - Fees: " + sub.subjectFees);
                        }

                        Console.Write("\nEnter Subject Code to Register: ");
                        string subCode = Console.ReadLine();

                        bool subjectFound = false;
                        foreach (Subject sub in foundStudent.regDegree.subjects)
                        {
                            if (sub.code == subCode)
                            {
                                foundStudent.regStudentSubject(sub);
                                subjectFound = true;
                                break;
                            }
                        }

                        if (!subjectFound)
                        {
                            Console.WriteLine("Subject not found!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Student not found or not registered in any program!");
                    }
                }
                else if (option == 7)
                {
                    // Calculate Fees for all Registered Students
                    Console.WriteLine("\nFees for Registered Students:");
                    foreach (Student stu in students)
                    {
                        if (stu.regDegree != null)
                        {
                            float fee = stu.calculateFee();
                            Console.WriteLine(stu.name + " (" + stu.regDegree.degreeName + ") - Fee: Rs. " + fee);
                        }
                    }
                }

                if (option != 8)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (option != 8);

            Console.WriteLine("Thank you for using UAMS!");
            Console.ReadKey();
        }
    }
}