# azYouTubeStats

Using the Serverless Azure functions it returns YouTube Statistics.

Once deployed, you will need to add `APIKEY` to you configuration. If you are running it locally add `local.settings.json` file with the correct values.

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "APIKEY": "__YOUR_API_KEY__"
  }
}
```
## Deployment

[Develop Azure Functions by using Visual Studio Code](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp&?WT.mc_id=azYouTubeStats-github-frbouche)

## Currently implemented:

More to come but for now this is what we have.

---

### GetVideoStatsFromPlaylist

Return the **real** statistics for all individual videos contain in a YouTube PlayList.

```
GET GetVideoStatsFromPlaylist 

Input:
    {
        // [Required]
        "playlistId": "xxxxxxxxxxxxxxxxxxx",

        // [Required]
        "code": "azure security token"
    }
Output:
    {
    "contentType": null,
    "serializerSettings": null,
    "statusCode": null,
    "value": [
        {
        "videoId": "xxxxxxxxxxx",
        "title": "Video Title",
        "viewCount": 903,
        "likeCount": 26,
        "dislikeCount": 4,
        "commentCount": 6,
        "language": "fr-CA",
        "tags": [
          "microsoft azure",
          "cloud"
        ],
        "publishedAt": "2021-03-01T11:41:27Z",
        "duration": "PT7M5S"
        }
    ]
}

```