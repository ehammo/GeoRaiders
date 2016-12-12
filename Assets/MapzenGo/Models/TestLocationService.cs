using UnityEngine;
using UnityEngine.UI;

public class TestLocationService : MonoBehaviour
{

    //public Text latitudeText;
    
    //public Text longitudeText;
    public float lat;
    public float lon;
    public float prevLat;
    public float prevLon;
    Text testando;
    public void calculatePos() {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            //yield break;
         //   testando.text = "gps nao ligado";
        }
        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            //yield return new WaitForSeconds(1);
          //  testando.text = "gps inicializando";
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            //yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
           // testando.text = "gps falhou";
            //yield break;
        }
        else
        {
            prevLat = lat;
            prevLon = lon;
            // Access granted and location value could be retrieved
            lat = Input.location.lastData.latitude;
            //latitudeText.text = "" + Input.location.lastData.latitude;
            lon = Input.location.lastData.longitude;
            //longitudeText.text = "" + Input.location.lastData.longitude;
           // testando.text = ("Location: " + lat + " " + lon );
            //Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            //debugLogText.text = "gps funcionou";
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    //IEnumerator Start()
    void Start() {
        Debug.Log("startei gps");
        /*
        //var v2 =  GM.LatLonToMeters(Latitude, Longitude);
        GameObject canvas = GameObject.FindGameObjectWithTag("testando").gameObject;
        testando = canvas.transform.FindChild("Text").GetComponent<Text>();

        calculatePos(); 
        */
    }

   // void Update() { callUpdate(); }
    public void callUpdate() {
        prevLat = lat;
        prevLon = lon;
        lat = Input.location.lastData.latitude;
        //latitudeText.text = "" + Input.location.lastData.latitude;
        lon = Input.location.lastData.longitude;
        //longitudeText.text = "" + Input.location.lastData.longitude;
    }

    public float getLat() { callUpdate(); return lat; }
    public float getLon() { callUpdate(); return lon; }
    public float getLatDesloc() { callUpdate(); return lat - prevLat; }
    public float getLonDesloc() { callUpdate(); return lon - prevLon; }

}