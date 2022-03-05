# azYouTubeStats

Using the Serverless Azure functions it returns YouTube Statistics.

Once deployed, you will need to add `APIKEY` to you configuration. If you are running it locally rename `local.settings.example.json` file `local.settings.json` and update `__YOUR_API_KEY__` with your value.

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


GET GetVideoStatsFromPlaylist 

Body Intput

```json

{
  // [Required]
  "playlistId": "xxxxxxxxxxxxxxxxxxx",

  // [Required]
  "code": "azure security token"

  // [Optional]
  "withComment": "true"
}

```

Body Output:
``` json
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
      "comments": [
        {
          "commentId": "1234567",
          "videoId": "xxxxxxxxxxx",
          "videoTitle": "Video Title",
          "textOriginal": "Thank you",
          "authorDisplayName": "Frank Boucher",
          "likeCount": 0,
          "totalReplyCount": 0,
          "publishedAt": "2021-01-10T23:02:59Z",
          "updatedAt": "2021-01-10T23:03:13Z",
          "isPublic": true
        },
        {
          "commentId": "12345678",
          "videoId": "xxxxxxxxxxx",
          "videoTitle": "Video Title",
          "textOriginal": "Thanks for sharing",
          "authorDisplayName": "Frank Boucher",
          "likeCount": 2,
          "totalReplyCount": 0,
          "publishedAt": "2020-11-12T13:14:15Z",
          "updatedAt": "2020-11-12T13:14:15Z",
          "isPublic": true
        }
      ],
      "publishedAt": "2021-03-01T11:41:27Z",
      "duration": "PT7M5S"
      }
  ]
}

```



### GetVideoCommentsFromPlaylist

Return the all comments of all the videos contain in a YouTube PlayList.


GET GetVideoCommentsFromPlaylist 
Body Input:

```json

{
    // [Required]
    "playlistId": "xxxxxxxxxxxxxxxxxxx",

    // [Required]
    "code": "azure security token"
}

```

Body Output:
```json
{
  "contentType": null,
  "serializerSettings": null,
  "statusCode": null,
  "value": [
    {
      "commentId": "1234567",
      "videoId": "xxxxxxxxxxx",
      "videoTitle": "Video Title",
      "textOriginal": "Thank you",
      "authorDisplayName": "Frank Boucher",
      "likeCount": 0,
      "totalReplyCount": 0,
      "publishedAt": "2021-01-10T23:02:59Z",
      "updatedAt": "2021-01-10T23:03:13Z",
      "isPublic": true
    },
    {
      "commentId": "12345678",
      "videoId": "zzzzzzzz",
      "videoTitle": "Another Video Title",
      "textOriginal": "Thanks for sharing",
      "authorDisplayName": "Frank Boucher",
      "likeCount": 2,
      "totalReplyCount": 0,
      "publishedAt": "2020-11-12T13:14:15Z",
      "updatedAt": "2020-11-12T13:14:15Z",
      "isPublic": true
    }
  ]
}

```