# azYouTubeStats

Using the Serverless Azure functions it returns YouTube Statistics.

## Currently implemented:

More to come but for now this is what we have.

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