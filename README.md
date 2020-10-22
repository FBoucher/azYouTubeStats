# azYouTubeStats

Using the Serverless Azure functions it returns YouTube Statistics.

Once deploy you will need to add `APIKEY` to you configuration. If you are running it locally add `local.settings.json` file with the correct values.

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
        "favoriteCount": 0,
        "commentCount": 6
        }
    ]
}

```