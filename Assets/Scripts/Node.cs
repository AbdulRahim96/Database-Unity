public class MYNode
{
    public string dataType;
    public MYNode next, prev;

    public string name, fullName, ID, Gender;

    public string data1, data2, data3;

    public MYNode(string type, string n, string fn, string id, string gender, string d1, string d2, string d3)
    {
        dataType = type;
        name = n;
        fullName = fn;
        ID = id;
        Gender = gender;
        next = null;
        prev = null;
        data1 = d1;
        data2 = d2;
        data3 = d3;
    }

    public string viewNode()
    {
        string details;
        details = dataType + "\n============================\n" + data1 + "\n" + data2 + "\n" + data3;
        return details;
    }

    public string viewSingleNode()
    {
        string details;
        details = dataType + "\nFirst Name: " + name + "\nLast Name: " + fullName + "\nID: " + ID + "\nGender: " + Gender + "\n" + data1 + "\n" + data2 + "\n" + data3;
        return details;
    }
    
}
