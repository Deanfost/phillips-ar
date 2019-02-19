# Phillips AR
An undergraduate research project investigating the uses of AR into painting data visualization.

## Phillips Virtual Culture @UMD
Phillips Virutal Culture is a research stream under UMD's First-year Innovation and Research Experience (FIRE), focused on bringing XR technologies to the museum space. This project is under the supervision of FIRE and in collaboration with The Phillips Collection in Washington DC. Work on this repository began in late Septmeber of 2018 and will continue until the end of the first semester.

## User Experience
Using Unity3D and Google AR Core, image targets of paintings are used to add worldspace anchors to hold 3D models of the respective painting. "Pieces" of the painting - each containing important people or objects - make up each model. When the full model is instantiated, it is anchored on the image target, allowing the user to seemingly interact with the painting itself. Currently the only image target supported is ["The Luncheon of the Boating Party"](https://en.wikipedia.org/wiki/Luncheon_of_the_Boating_Party) by Pierre-Auguste Renoir.

Alongside the model, UI cards are displayed in the worldspace, containing model controls and information on the work, such as year of work, name, artist, and descriptions. Upon interacting with a piece, it separates from the model, flying forward and placing focus on itself. Context cards appear nearby, showing information to the user specific to that piece. 

![Screenshot 1](https://github.com/Deanfost/phillips-ar/blob/master/Screenshots/Screenshot_20181124-165008.jpg)
![Screenshot 2](https://github.com/Deanfost/phillips-ar/blob/master/Screenshots/Screenshot_20181124-165033.jpg)

### Tracking Issues
It should be noted that during runtime, the model may shift around and rescale. This is most likely due to AR Core's inability to track paintings as accurately as normal images. In the [documentation,
](https://developers.google.com/ar/develop/unity/augmented-images/) it states that images with scores of 75/100 (using AR Core's built-in tool) or above are tracked accurately; the current image target recieves a score of 0/100, with most other painting targets attempted recieving a similar score. Even when attempted with targets with very few repeating features, the result was still consistent with a less than ideal score.

## Technical Details
### Libraries and Technologies Used
- [Unity3D](https://unity3d.com/)
- [Google AR Core](https://developers.google.com/ar/)
- [JSONObject](https://github.com/mtschoen/JSONObject)

### Device Requirements
Currently the project is compiling for Android devices. Android devices are required to [support AR Core](https://developers.google.com/ar/discover/supported-devices) with a minimum API level of 24. 
