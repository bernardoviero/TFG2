using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SMAConnection : MonoBehaviour
{
    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;
    private bool isConnected = false;
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 12345;

    [Serializable]
    public class AirplaneData
    {
        public string airplaneName;
        public float positionX;
        public float positionY;
        public bool isLanding;
        public bool isTakingOff;
    }

    void Start()
    {
        StartCoroutine(ConnectToServer());
    }

    private IEnumerator ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverAddress, serverPort);
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            isConnected = true;
            Debug.Log("Conectado ao servidor SMA.");

            StartCoroutine(ReceiveDataFromServer());
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao conectar ao servidor: " + e.Message);
        }

        yield return null;
    }

    private IEnumerator ReceiveDataFromServer()
    {
        while (isConnected)
        {
            if (client.Available > 0)
            {
                string jsonData = reader.ReadLine();
                if (!string.IsNullOrEmpty(jsonData))
                {
                    ProcessJsonData(jsonData);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ProcessJsonData(string jsonData)
    {
        try
        {
            List<AirplaneData> airplanes = JsonConvert.DeserializeObject<List<AirplaneData>>(jsonData);

            foreach (var airplane in airplanes)
            {
                ApplyAnimation(airplane);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao processar JSON: " + e.Message);
        }
    }

    private void ApplyAnimation(AirplaneData airplane)
    {
        GameObject airplaneObject = GameObject.Find(airplane.airplaneName);
        if (airplaneObject != null)
        {
            airplaneObject.transform.position = new Vector2(airplane.positionX, airplane.positionY);

            if (airplane.isLanding)
            {
                airplaneObject.GetComponent<AirplaneMovement>().StartLandingAnimation();
            }
            else if (airplane.isTakingOff)
            {
                airplaneObject.GetComponent<AirplaneMovement>().StartTakeoffAnimation();
            }

            SendAnimationDataToServer(airplane);
        }
        else
        {
            Debug.LogWarning($"Avião {airplane.airplaneName} não encontrado na cena.");
        }
    }

    private void SendAnimationDataToServer(AirplaneData airplane)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(airplane);
            writer.WriteLine(jsonData);
            Debug.Log("Dados de animação enviados para o servidor: " + jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao enviar dados para o servidor: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Close();
        }
    }
}