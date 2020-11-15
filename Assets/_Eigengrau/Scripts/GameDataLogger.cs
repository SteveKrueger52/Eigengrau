using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using GData = Google.Apis.Sheets.v4.Data;
using GAppendRequest = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.AppendRequest;

public class GameDataLogger : MonoBehaviour
{
    #region KEY

    private string key =
        "{\n  \"type\": \"service_account\",\n  \"project_id\": \"eigengrau\",\n  \"private_key_id\": \"e913c812d34e093154cec54067dca275f46ca05b\",\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDMzcWs0MwA4ZXZ\\nnx5tQ6JniuYeRg+5iUmTvmlY5kgVIPgISBPdrvhEzjAMFTbH2WcLKeOfm0+VPGaS\\nrbr4FSjJYk26CdpcMHt2LNNQnxzL/S+46QeaIf7mcfbK0wt6OEiGyEYKvq5j3d2b\\nd7sNHO6cmAvo9MKy5KyzQG114wox+B0f6mDj8vq0JAh2Aq+zhih7T+RGRZc19ach\\nvqlKZru4Mus/cApYdUgZ707oc+3xa/19QjVJ2Pv6ngaupFw7my28H4Y8n4qQ0KBc\\n2Lvqkr+fY6QuJpQGM/ryys6Ou+J6NDi8cE90kK+N1bAzxKJM3SJ0u5Tu5gBzk7/J\\nV0MxpV0xAgMBAAECggEABDgv12V1SNtySc7/1UCryUIfnI4ag4rWSUuxWibt7fdY\\nEYccd4Bz/jUTi4yEbI9bB9we7oDobbF2W/VQenfzTKazOwxPi3SmrDF58HxOZK6Y\\nj8DSKAEP7O26Mn/w6Z2R3crajxInmoMAXnmZNj4jVXZ4Oy5PmdhmYrp2xYimGHph\\nRu62cm3SDW/altV/T+vdV9MluDz9dMu37kBKGvo8qbmYdqJcTqJVcLUM2HgUnWfr\\nYeIBuVpmtDsyAq/nzbM/IaIfr0F/ZIBEmTZY98o1LowUxVv2M3CcV2HzV2BNGagl\\n3jwMxyyOh6bUnXnoOgETQDtAteyqq3fEbSyGISxz4QKBgQDt2tgNbYT4BHKqtdnk\\nrfgGz3W4nLYv8ZfFs+P16PMxwtMv3SJ1/rcOm64i5M88aFN6iiyVPuj9ZGZ8maZD\\njVbRveqDkXmD8Cfro+t5DLWG8dzIqP5BYW4V/FWVTMRjEp2BZ2yp78g22U+NQ02G\\neogLB/wen1Mv71yjGNS9klJo0QKBgQDcbXY0GtK81oXUMpa1Sek6+WtNjAOpLU4v\\nG0xVE5CHULcAff1wM5+A65I4Aa6vztprQ36sabJYP8mZHzodGbY9l+D3ZP+ctsWf\\n174fuLykxmy/Th1FdScSrT3cuTlmLCV2ZcIAAEgnhJgY/5bfCnSMQ+ysIHlQxv9C\\nlqm0BDrGYQKBgFJg2zc7WTQ1BinNtduXzTE+pPz9ebP9Lp2QgkFnHuN7IH2RTyAi\\nye/LR3JaYNQBJfho5qwCD9vC7CP3azg2OX2HEzPiLH3frVPVf/Z6ylwkZF0y+mxW\\nfDQtIs8EuXe+uMJaua3ZmDQ263B3gOA0i+2WJGHOuJb/hcwPC49koBohAoGBAJhh\\nD1q7xD0I66Eii66tdo7vR29nQEyeZjMIYv9ZNAtnD+tGLYJ4HWao8v7GovUkQTfv\\ng806kDCTgBDbVngxgxsXOEytxEqYywtRwDv7oGUkSp8BKBkRAL7XhjFk1jo8gHVV\\nqQQJLdgrwF1fqvNrLeCdQCpJlLPzrRhiQNSSK/NhAoGBAOkX1YWXSu1ogO1CAWaF\\nLOdmO5SN+zAzqolroRT40+2Zm7knvXAtc2l1bOz256Us6AXOVVR6Z2h4VYoXt/Ks\\nFzZ1NChP6OFtF2YTotGSa6VfIle9RONIZbuPA0XVhBF4ydgLn5hBTXm53QqS8C8a\\nR0Z7tvykKgM5Ab6UEhga1JNo\\n-----END PRIVATE KEY-----\\n\",\n  \"client_email\": \"eigengrau-master@eigengrau.iam.gserviceaccount.com\",\n  \"client_id\": \"112253759813482404296\",\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/eigengrau-master%40eigengrau.iam.gserviceaccount.com\"\n}\n";
    #endregion
        
    private static GameDataLogger _instance;

    public static GameDataLogger Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/SceneChanger") as GameObject);
                _instance = go.GetComponent<GameDataLogger>();
            }

            return _instance;
        }
    }
    
    private IList<DateTime> _timestamps;
    private IList<int> _drawingOrder;
    private IList<bool> _drawingsCompleted;
    
    private SheetsService _sheetsService;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
            Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _timestamps = new List<DateTime> {DateTime.Now};
        _drawingOrder = new List<int>();
        _drawingsCompleted = new List<bool>();
    }
    
    // ------------------------------------------------------------------------------------------------
    // LOGGING METHODS
    // ------------------------------------------------------------------------------------------------

    private string DateTimesToString(DateTime start, DateTime end)
    {
        TimeSpan range = end.Subtract(start);
        return (range.Hours != 0 ? range.Hours + "h" : "") +
                        range.Minutes + "m" + range.Seconds + "." + range.Milliseconds + "s";
    }
    
    public static void EnterDrawScene(int drawIndex)
    {
        Instance._timestamps.Add(DateTime.Now);
        Instance._drawingOrder.Add(drawIndex);
    }

    public static void ExitDrawingScene(bool saved)
    {
        Instance._timestamps.Add(DateTime.Now);
        Instance._drawingsCompleted.Add(saved);
    }

    private IList<IList<object>> Compile()
    {
        IList<IList<object>> result = new List<IList<object>>();
        
        for (int i = 0; i < 17; i++)
            result.Add(new List<object>());
        
        for (int i = 0; i < _drawingOrder.Count; i++)
        {
            // Column C: In-Between Drawings
            result[0].Add(DateTimesToString(_timestamps[i*2],_timestamps[i*2 +1]));
            
            // Drawings: Time Column
            result[_drawingOrder[i] * 2 + 1].Add(DateTimesToString(_timestamps[i*2 +1],_timestamps[i*2 +2]));
            
            // Drawings: Saved Column
            result[_drawingOrder[i] * 2 + 2].Add(_drawingsCompleted[i]);
        }
        
        // Final timestamp (between last drawing and app close)
        result[0].Add(DateTimesToString(_timestamps[_drawingOrder.Count * 2],_timestamps[_drawingOrder.Count * 2 +1]));
        
        // Column A: Total Drawings
        result.Insert(0, new List<object>{_drawingOrder.Count});
        
        // Column B: Drawing Order
        result.Insert(1, new List<object>{String.Join("",_drawingOrder)});
        
        return result;
    }

    // ------------------------------------------------------------------------------------------------
    // GSheets API Stuff
    // ------------------------------------------------------------------------------------------------

    public void BeforeApplicationQuit()
    {
        _timestamps.Add(DateTime.Now);

        while (_drawingsCompleted.Count < _drawingOrder.Count)
            _drawingsCompleted.Add(false);

        IList<IList<object>> data = Compile();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }

        Debug.Log("Connecting to Google...");
        ConnectToGoogle();
        Debug.Log("Connected to Google; Posting Dummy Data");
        PostToGoogle(new [] {new object[] {"TEST"}});
        Debug.Log("Posting actual Data");
        PostToGoogle(data);
        Debug.Log("All Done!");
    }

    private void ConnectToGoogle()
    {
        GoogleCredential credential =
            //GoogleCredential.FromJson(key);
            //GoogleCredential.FromJson(Resources.Load<TextAsset>("GSheetsAPI").text);
            //GoogleCredential.FromFile(Application.streamingAssetsPath + "/key.json");
            GoogleCredential.FromStream(new FileStream(Application.streamingAssetsPath + "/key.json", FileMode.Open));


        _sheetsService = new SheetsService(new BaseClientService.Initializer {
            HttpClientInitializer = credential, ApplicationName = "Eigengrau"
        });
    }

    private void PostToGoogle(IList<IList<object>> data)
    {
        // The ID of the spreadsheet to update.
        string spreadsheetId = "14CQv-KsLjUweeZS9Am-dftJBlaGAiFGpwRJb8CK-tJQ";

        // The A1 notation of a range to search for a logical table of data.
        // Values will be appended after the last row of the table.
        string range = "GameData!A1:Y";

        // How the input data should be interpreted.
        GAppendRequest.ValueInputOptionEnum valueInputOption = GAppendRequest.ValueInputOptionEnum.USERENTERED;

        // How the input data should be inserted.
        GAppendRequest.InsertDataOptionEnum insertDataOption = GAppendRequest.InsertDataOptionEnum.INSERTROWS;

        // TODO: Assign values to desired properties of `requestBody`:
        GData.ValueRange requestBody = new GData.ValueRange();
        requestBody.Values = data;
        requestBody.MajorDimension = "COLUMNS";

        GAppendRequest request = _sheetsService.Spreadsheets.Values.Append(requestBody, spreadsheetId, range);
        request.ValueInputOption = valueInputOption;
        request.InsertDataOption = insertDataOption;

        // To execute asynchronously in an async method, replace `request.Execute()` as shown:
        GData.AppendValuesResponse response = request.Execute();
        // GData.AppendValuesResponse response = await request.ExecuteAsync();

        // TODO: Change code below to process the `response` object:
        Console.WriteLine(JsonConvert.SerializeObject(response));
    }
}
