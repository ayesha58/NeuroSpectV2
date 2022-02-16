
mergeInto(LibraryManager.library, {
    InsertData: function (tableName, code, age, race, gender, attentionData, recallData, visualData, attentionScore, recallScore, visualScore, timestamp) {

        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "code": Pointer_stringify(code),
                "age": Pointer_stringify(age),
                "race": Pointer_stringify(race),
                "gender": Pointer_stringify(gender),
                "attentionData": Pointer_stringify(attentionData),
                "recallData": Pointer_stringify(recallData),
                "visuospatialData": Pointer_stringify(visualData),
                "attentionScore": attentionScore,
                "recallScore": recallScore,
                "visuospatialScore": Pointer_stringify(visualScore),
                "timestamp": timestamp
            }
        }

        var awsConfig =
        {
            region: "us-east-2",
            endpoint: "https://dynamodb.us-east-2.amazonaws.com",
            accessKeyId: "AKIAZJJQRA2HH3T3FMGK",
            secretAccessKey: "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"
        }


        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function (err, data) {
            if (err) {
                returnStr = "Error: " + JSON.stringify(err, undefined, 2);
                SendMessage('Main Camera', 'StringCallback',
                    returnStr);
            }
            else {
                returnStr = "Inserted: " + JSON.stringify(data, undefined,
                    2);
                SendMessage('Main Camera', 'StringCallback',
                    returnStr);
            }
        });
    }
});