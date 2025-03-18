//using System.Collections;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class AnimatedUIBackground : MonoBehaviour
//{
//    public Sprite[] spriteFrames; // Assign sprite frames in Inspector
//    public float frameRate = 0.1f; // Control animation speed

//    private VisualElement background;
//    private int currentFrame = 0;



//    //void Start()
//    //{
//    //    var root = GetComponent<UIDocument>().rootVisualElement;
//    //    background = root.Q<VisualElement>("CQYS_startPage"); // Ensure this matches your UI name

//    //    if (background == null)
//    //    {
//    //        Debug.LogError("Error: Could not find 'Background' VisualElement! Check UI Builder.");
//    //        return;
//    //    }
//    //    else
//    //    {
//    //        Debug.Log("✅ Found 'Background' element in UI.");
//    //    }

//    //    if (spriteFrames == null || spriteFrames.Length == 0)
//    //    {
//    //        Debug.LogError("Error: No sprite frames assigned in Inspector!");
//    //        return;
//    //    }

//    //    Debug.Log("Background element found! Starting animation...");
//    //    StartCoroutine(AnimateBackground());
//    //}

//    void Start()
//    {
//        var root = GetComponent<UIDocument>().rootVisualElement;
//        background = root.Q<VisualElement>("CQYS_startPage");

//        if (spriteFrames.Length > 0 && background != null)
//        {
//            Debug.Log("Manually setting test frame...");
//            background.style.backgroundImage = new StyleBackground(spriteFrames[0].texture);
//            background.MarkDirtyRepaint(); // Force UI refresh
//        }
//        else
//        {
//            Debug.LogError("❌ spriteFrames missing OR Background not found!");
//        }
//    }



//    //void OnEnable()
//    //{
//    //    StartCoroutine(AnimateBackground());
//    //}

//    //void OnDisable()
//    //{
//    //    StopAllCoroutines();
//    //}

//    void Update()
//    {
//        if (!this.gameObject.activeInHierarchy)
//        {
//            Debug.LogError("🚨 GameObject got disabled! Animation stopped.");
//        }
//    }



//    //IEnumerator AnimateBackground()
//    //{
//    //    while (true)
//    //    {
//    //        Debug.Log("Animating frame: " + currentFrame);
//    //        background.style.backgroundImage = new StyleBackground(spriteFrames[currentFrame].texture);
//    //        currentFrame = (currentFrame + 1) % spriteFrames.Length;
//    //        yield return new WaitForSeconds(frameRate);
//    //    }
//    //}

//    //IEnumerator AnimateBackground()
//    //{
//    //    while (true)
//    //    {
//    //        Debug.Log($"Animating frame: {currentFrame} / {spriteFrames.Length}");

//    //        if (spriteFrames == null || spriteFrames.Length == 0)
//    //        {
//    //            Debug.LogError("Error: No sprite frames assigned!");
//    //            yield break;
//    //        }

//    //        if (spriteFrames[currentFrame] == null)
//    //        {
//    //            Debug.LogError($"Error: spriteFrames[{currentFrame}] is null!");
//    //            yield break;
//    //        }

//    //        background.style.backgroundImage = new StyleBackground(spriteFrames[currentFrame]);


//    //        currentFrame = (currentFrame + 1) % spriteFrames.Length;
//    //        yield return new WaitForSeconds(frameRate);
//    //    }
//    //}

//    IEnumerator AnimateBackground()
//{
//    Debug.Log("Starting background animation...");

//    while (true)
//    {
//        Debug.Log($"Updating Background to frame {currentFrame} / {spriteFrames.Length}");

//        if (spriteFrames == null || spriteFrames.Length == 0)
//        {
//            Debug.LogError("Error: No sprite frames assigned!");
//            yield break;
//        }

//        if (spriteFrames[currentFrame] == null)
//        {
//            Debug.LogError($"Error: spriteFrames[{currentFrame}] is null!");
//            yield break;
//        }

//        Texture2D frameTexture = spriteFrames[currentFrame].texture;
//        if (frameTexture == null)
//        {
//            Debug.LogError("Error: Converted texture is NULL!");
//            yield break;
//        }
//        if (spriteFrames[currentFrame].texture == null)
//        {
//            Debug.LogError($"🚨 Texture is NULL for frame {currentFrame}!");
//        }
//        else
//        {
//            Debug.Log($"✅ Texture loaded successfully: {spriteFrames[currentFrame].texture.name}");
//        }

//        Debug.Log($"✅ Applying new frame {currentFrame} to background.");

//        background.style.backgroundImage = null;
//        yield return null; // Force a UI update frame

//        background.style.backgroundImage = new StyleBackground(frameTexture);
//        background.MarkDirtyRepaint(); // Ensure UI updates

//        Debug.Log($"⏳ Waiting {frameRate} seconds before next frame...");
//        yield return new WaitForSeconds(frameRate);

//        currentFrame = (currentFrame + 1) % spriteFrames.Length;
//    }
//}
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    public Image m_Image;

    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;

    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;
    public void Func_PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(Func_PlayAnimUI());
    }
    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsDone == false)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
}