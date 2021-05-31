using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBG : MonoBehaviour
{
    public List<Button> mBGBTList;
    public List<Texture> mBGList;
    public RawImage mImage;
    // Start is called before the first frame update
    void Start()
    {

     //  Screen.SetResolution(1375, 750,FullScreenMode.FullScreenWindow);
        for (int i = 0; i < mBGBTList.Count; i++ )
        {
            int index = i;
            mBGBTList[i].onClick.AddListener(() => {
                OnClick(index);
            });
        }
    
    }

    private void OnClick(int i )
    {
        mImage.texture = mBGList[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
