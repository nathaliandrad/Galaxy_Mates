using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Vuforia;
using System.Collections.Generic;
    
public class ComplexARCloudHandler : MonoBehaviour, IObjectRecoEventHandler
{
// The ImageTarget that contains our 3D objects
public ImageTargetBehaviour imageTargetTemplate;
// Vuforia's Cloud Recognition object
private CloudRecoBehaviour cloudRecoBehaviour;
// The Vuforia ObjectTracker
private ObjectTracker objectTracker;
// To keep track of when we are scanning
private TargetFinder targetFinderObj;

private bool isScanning = false;
// Holds the imageURLs we will load later
private string[] imageURLs;
// The index of the image being shown
private int imageIndex;
// Reference to the Raw Image object in our scene
private RawImage image;

//Audio
public AudioSource speaker;

void Start()
{
    speaker = GetComponent<AudioSource>();
    speaker.Play(1);
    imageIndex = 0;
this.cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
if (this.cloudRecoBehaviour)
{
this.cloudRecoBehaviour.RegisterEventHandler(this);
}
}

public void OnInitialized(TargetFinder targetFinder)
{
Debug.Log("Cloud Reco initialized");
// Get reference to the object tracker
targetFinderObj = targetFinder;
objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
}
public void OnInitError(TargetFinder.InitState initError)
{
Debug.Log("Cloud Reco init error " + initError.ToString());
switch (initError)
{
case TargetFinder.InitState.INIT_ERROR_NO_NETWORK_CONNECTION:
Debug.Log("Network unavailable. Please check your internet connection");
break;
case TargetFinder.InitState.INIT_ERROR_SERVICE_NOT_AVAILABLE:
Debug.Log("Service unavailable. Failed to initialize.");
break;
}
}

public void OnUpdateError(TargetFinder.UpdateState updateError)
{
Debug.Log("Cloud Reco update error " + updateError.ToString());
}
// This method lets you know whether Vuforia is scanning the cloud.
public void OnStateChanged(bool scanning)
{
isScanning = scanning;
if (scanning)
{
// clear all known trackables
//objectTracker.TargetFinder.ClearTrackables(false);
targetFinderObj.ClearTrackables(false);
}
}
// This is used along our ImageTarget template, which we duplicate each time
// then delete all objects in it that we don't need
private GameObject destroyAllGameObjectsExcept(Transform parentTransform, string gameObjectName)
{
GameObject matchingGameObject = null;
foreach (Transform item in parentTransform)
{
if (item.name != gameObjectName)
{
Destroy(item.gameObject);
}
else
{
matchingGameObject = item.gameObject;
}
}
return matchingGameObject;
}


// Here we handle a cloud target recognition event
public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
{
TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;

// hang on to the metadata so we can use it later
var metaData = JsonUtility.FromJson<MetaData>(cloudRecoSearchResult.MetaData);
if (metaData != null)
{
// duplicate the referenced image target
GameObject newImageTarget = Instantiate(imageTargetTemplate.gameObject) as GameObject;
//ImageTargetBehaviour imageTargetBehaviour = objectTracker.TargetFinder.EnableTracking(targetSearchResult, newImageTarget);
//TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;
TrackableBehaviour imageTargetBehaviour = targetFinderObj.EnableTracking(targetSearchResult, newImageTarget);

GameObject visibleGameObject = this.destroyAllGameObjectsExcept(imageTargetBehaviour.gameObject.transform, metaData.name);
this.image = visibleGameObject.GetComponentInChildren<RawImage>();
//Question Check
if (metaData.type == "image")
{
if (metaData.urls != null)
{
this.imageURLs = metaData.urls;
}
this.renderImage();
}
String wrongAnswerImg = "https://i.ibb.co/YNLG6B6/71836246-2814824711884592-4347226473782837248-o.jpg";
String correctAnswerImg = "https://i.ibb.co/7SD1r6W/71499612-2814824778551252-2924664774849462272-o.jpg";

//Answer Check
switch(this.imageIndex) {
    case 0: {
        if (metaData.type == "answer" && metaData.answer == "Earth") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 1: {
        if (metaData.type == "answer" && metaData.answer == "Mars") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 2: {
        if (metaData.type == "answer" && metaData.answer == "Jupiter") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 3: {
        if (metaData.type == "answer" && metaData.answer == "Mercury") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 4: {
        if (metaData.type == "answer" && metaData.answer == "Venus") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 5: {
        if (metaData.type == "answer" && metaData.answer == "Uranus") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 6: {
        if (metaData.type == "answer" && metaData.answer == "Saturn") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    case 7: {
        if (metaData.type == "answer" && metaData.answer == "Neptune") {
            this.renderSelectedImage(correctAnswerImg);
            this.showNextQuestion();
        } else if (metaData.type == "answer") {
            this.renderSelectedImage(wrongAnswerImg);
        }
        break;
    }

    default:
        break;
}

}
}


IEnumerator downloadImage(string url, Action<Texture2D> result)
{

using (WWW www = new WWW(url))
{
// Wait for download to complete
yield return www;
result(www.texture);
}
}
public void renderImage() {
    var imageURL = this.imageURLs[this.imageIndex];
    StartCoroutine(downloadImage(imageURL, value => this.image.texture = value));
}
public void renderSelectedImage(String imageToRender) {
    StartCoroutine(downloadImage(imageToRender, value => this.image.texture = value));
}
public void showNextQuestion()
{
this.imageIndex++;
}
public void showPreviousQuestion()
{
if (this.imageIndex > 0)
{
this.imageIndex--;
var imageURL = this.imageURLs[this.imageIndex];
StartCoroutine(downloadImage(imageURL, value => this.image.texture = value));
}
}

 

}

 

[Serializable]
public class MetaData
{
public string type;
public string name;
public string answer;
public string url; // for when we use metadata with singular URL
public string[] urls; // for when we use metadata with multiple URLs
}