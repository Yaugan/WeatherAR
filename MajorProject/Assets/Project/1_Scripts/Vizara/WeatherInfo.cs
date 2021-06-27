using System;
using System.Collections.Generic;


[Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;
}

[Serializable]
public class Main
{
    public float temp;
    public float feels_like;
    public int humidity;
    public float pressure;
}

[Serializable]
public class Sys
{
    public string country;
}



[Serializable]
public class WeatherInfo
{
    public string name;
    public List<Weather> weather;
    public Main main;
    public Sys sys;
}




