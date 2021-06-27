using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour
{
    private const string API_KEY = "0e244bf202af92fe0dcef5f049d38946"; 
    public string CityId;

    //UI 
    public Text nameCity;
    public Text temp;
    public Text description;
    public Text hum;
    public Text pres;
    public Image weatherImage;
    public Sprite rain, thunderstorm, drizzle, snow, atm, clear, clouds;

    public GameObject tutorialPanel;
    public GameObject weatherPanel;

    void Start()
    {
        StartCoroutine(GetWeather(CheckSnowStatus));
    }
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Location1")
                {
                    CityId = "1273294";
                    StartCoroutine(GetWeather(CheckSnowStatus));
                }
                else if (hit.collider.tag == "Location2")
                {
                    CityId = "2147714";
                    StartCoroutine(GetWeather(CheckSnowStatus));
                }
                else if (hit.collider.tag == "Location3")
                {
                    CityId = "5128581";
                    StartCoroutine(GetWeather(CheckSnowStatus));
                }
            }
        } 
    }


    public void CheckSnowStatus(WeatherInfo weatherObj)
    {
        weatherPanel.SetActive(true);
        nameCity.text = weatherObj.name + ", " + weatherObj.sys.country;
        var x = weatherObj.main.temp;
        temp.text = ((int)(x - 273)).ToString() + "°C";
        description.text = "Feels like " + ((int)weatherObj.main.feels_like - 273).ToString() + "°C. " + weatherObj.weather[0].description.ToUpper() + ".";
        hum.text = "Humidity : " +  weatherObj.main.humidity.ToString() + " %";
        pres.text = "Atm Pressure : " + weatherObj.main.pressure.ToString() + " hPa";

        if(weatherObj.weather[0].id >= 200 && weatherObj.weather[0].id <= 299)
        {
            weatherImage.sprite = thunderstorm;
        }
        else if(weatherObj.weather[0].id >= 300 && weatherObj.weather[0].id <= 399)
        {
            weatherImage.sprite = drizzle;
        }
        else if (weatherObj.weather[0].id >= 500 && weatherObj.weather[0].id <= 599)
        {
            weatherImage.sprite = rain;
        }
        else if (weatherObj.weather[0].id >= 600 && weatherObj.weather[0].id <= 699)
        {
            weatherImage.sprite = snow;
        }
        else if (weatherObj.weather[0].id >= 700 && weatherObj.weather[0].id <= 799)
        {
            weatherImage.sprite = atm;
        }
        else if (weatherObj.weather[0].id == 800)
        {
            weatherImage.sprite = clear;
        }
        else if (weatherObj.weather[0].id >= 800)
        {
            weatherImage.sprite = clouds;
        }
    }

    bool IsEmpty()
    {
        if (int.Parse(CityId) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    IEnumerator GetWeather(Action<WeatherInfo> onSuccess)
    {
        yield return new WaitUntil(IsEmpty);
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY)))
        {
            yield return req.SendWebRequest();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
           
            string weatherJSON = System.Text.Encoding.Default.GetString(result);
            WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
           
            onSuccess(info);
        }
    }

    public void Tutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void close()
    {
        tutorialPanel.SetActive(false);
    }

}
