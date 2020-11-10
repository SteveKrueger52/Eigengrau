using System;
using System.Collections;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using GData = Google.Apis.Sheets.v4.Data;
using GAppendRequest = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.AppendRequest;

public class TestSheetAPI : MonoBehaviour
{
    private DateTime _timeStart;
    private SheetsService _sheetsService;
    
    // Test references for dummy data
    public Text cell1;
    public Text cell2;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeStart = DateTime.Now;
        ConnectToGoogle();
    }

    private void OnApplicationQuit()
    {
        TimeSpan range = DateTime.Now - _timeStart;
        string toPost = range.Minutes + "m" + range.Seconds + "." + range.Milliseconds + "s";

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }

        PostToGoogle(new List<object> {"End Time:", toPost});
        
    }

    public void SendLine()
    {
        PostToGoogle(new List<object>{"Cell Contents", cell1.text, cell2.text});
    }
    
    private void ConnectToGoogle()
    {
        
        GoogleCredential credential = GoogleCredential.FromJson(Resources.Load<TextAsset>("GSheetsAPI").text);
        _sheetsService = new SheetsService(new BaseClientService.Initializer {
            HttpClientInitializer = credential, ApplicationName = "Eigengrau"
        });
    }

    private void PostToGoogle(List<object> row)
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
        requestBody.Values = new List<IList<object>> {row};

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
