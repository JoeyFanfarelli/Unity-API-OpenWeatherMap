using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Net;

public class WeatherScript : MonoBehaviour
{

    private const string API_KEY = "111";                               //My API key has been removed for privacy purposes. Please generate your own OpenWeatherMap API key if you would like to use this code.
    [SerializeField] private string zip = "12601";                      //The zip code that will be used when querying OpenWeatherMap's API for the weather.

    [SerializeField] private GameObject weatherParticles;               //Particle effect that will be manipulated based on the current weather.
    
    void Start()
    {

        //The following line...
        //Creates a new string named weather. 
        //Calls the GetWeather() function. This function returns an object which is made using the WeatherInfo class.
        //The WeatherInfo class has a variable called "weather", which corresponds to the key in the JSON file. Although weather is a list, it is only a list of one item - so we use [0]. 
        //Within that, there is id, main, description and icon. "main" contains the actual weather. 
        //The JSON file within weather looks like this

        //weather: [                <--weather
        //   -  {                   <--index [0] of the weather list
        //         id: 100,
        //         main: "Clouds",  <--main contains the actual weather.
        //         description: "broken clouds",
        //         icon: "04d",
        //      }
        //]
        
        string weather = GetWeather().weather[0].main;                          //Obtain current weather from API

        var weatherPS = weatherParticles.GetComponent<ParticleSystem>().main;   //Cache reference to particle system.
        
        //Change color of particle system based on the current weather (as returned by API).
        if (weather == "Clouds")                                                
        {
            weatherPS.startColor = new Color(1, 0, 0, .5f);                     
        }
        else if (weather == "Clear")
        {
            weatherPS.startColor = new Color(0, 0, 1, .5f);                     
        }


        Debug.Log(weather);                                                     //put the weather in the console so we can make sure we're getting it correctly.
    }

    void Update()
    {
        
    }



    [System.Serializable]   //JSONUtility won't work with the API unless we serialize (turn into string of bytes) this class.
    public class Weather    //These correspond to all of the sub-keys in the JSON file, within the weather key. They are not all used. Currently only using the main key, but i've included the rest for when I'm ready to use them.
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }
   

    public class WeatherInfo    //First level keys (coord, weather, base, main, visibility, wind...id, name, cod.) Not all are listed here. In order to use them, you would need to list them. Currently, id and name are not used.
    {
        public int id;
        public string name;             
        public List<Weather> weather;   //weather, here, uses the weather class that is defined above. That weather class had an int "id" and a string "main"
    }


    //HTTP Request to the OpenWeatherMap API, receives a response, converts it to JSON, and then returns the JSON.
    private WeatherInfo GetWeather()    
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?zip=" + zip + "&APPID=" + API_KEY));  
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();              
        StreamReader reader = new StreamReader(response.GetResponseStream());           
        string unformattedData = reader.ReadToEnd();                                    
        WeatherInfo jsonInfo = JsonUtility.FromJson<WeatherInfo>(unformattedData);      
        return jsonInfo;
    }

    
}
