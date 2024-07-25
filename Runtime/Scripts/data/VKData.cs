using System;
using System.Collections.Generic;

[Serializable]
public class VKData
{
    public string method;
    public string data;
    public string error;
    public string name;
    public int age;
    public bool isStudent;
    public List<int> scores;
    public Address address;

    [Serializable]
    public class Address
    {
        public string city;
        public string zip;
    }
}