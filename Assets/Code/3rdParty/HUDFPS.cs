using UnityEngine;
using System.Collections;

[AddComponentMenu( "Utilities/HUDFPS" )]
public class HUDFPS: MonoBehaviour
{
    // Attach this to any object to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // corstartRect overall FPS even if the interval renders something like
    // 5.5 frames.


    public static float CurrentFps { get; private set; }
    public static float AvarageFps { get; private set; }
    public static float LowestFps { get; private set; }
    public static float HighestFps { get; private set; }

    static bool _firstValue = true;

    // public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
    public float frequency = 0.5F; // The update frequency of the fps
    public int nbDecimal = 1; // How many decimal do you want to display

    Rect _startRect = new Rect( 10, 10, 300, 25 ); // The rect the window is initially displayed at.
    float _avgFactor = 0.9f;
    float _accum     = 0f; // FPS accumulated over the interval
    int   _frames    = 0; // Frames drawn over the interval
    // Color color     = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
    // string sFPS = ""; // The fps formatted into a string.
    GUIStyle _style; // The style the text will be displayed at, based en defaultSkin.label.


    public static void ResetCounter()
    {
        //Debug.Log( "Reseting FPS counter: "
        //    + HUDFPS.CurrentFps + " avg " + HUDFPS.AvarageFps
        //    + " min " + HUDFPS.LowestFps + " max " + HUDFPS.HighestFps
        //);

        _firstValue = true;
        AvarageFps = 0f;
        LowestFps = float.MaxValue;
        HighestFps = 0f;
    }

    void Start()
    {
        ResetCounter();
        StartCoroutine( FPS() );
    }


    void Update()
    {
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;
    }


    IEnumerator FPS()
    {
        // Infinite loop executed every "frenquency" secondes.
        while ( true ) {

            // Update the FPS
            float fps = _accum / _frames;

            //Update the color
            // color = ( fps >= 30 ) ? Color.green : ( ( fps > 10 ) ? Color.red : Color.yellow );

            if ( _firstValue && ( fps >= 0f || fps < 666f ) ) {
                AvarageFps = fps;
                _firstValue = false;
            }

            CurrentFps = fps;
            AvarageFps = AvarageFps * _avgFactor + CurrentFps * ( 1f - _avgFactor );
            LowestFps = Mathf.Min( LowestFps, CurrentFps );
            HighestFps = Mathf.Max( HighestFps, CurrentFps );

            _accum = 0.0F;
            _frames = 0;

            yield return new WaitForSeconds( frequency );
        }
    }

    void OnGUI()
    {
        // Copy the default label skin, change the color and the alignement
        if ( _style == null ) {
            _style = new GUIStyle( GUI.skin.label );
            _style.normal.textColor = Color.white;
            _style.alignment = TextAnchor.UpperLeft;
            _style.fontSize = 12;
        }

        // startRect = GUI.Window( 0, startRect, DoMyWindow, "" );
        GUI.Label(
            new Rect( 0, 0, _startRect.width, _startRect.height ),
            FloatToText( CurrentFps ) + " FPS avg " + FloatToText( AvarageFps )
                + " min " + FloatToText( LowestFps ) + " max " + FloatToText( HighestFps ),
            _style
        );

        //if ( Input.GetKey( Constants.INPUT_DEBUG ) ) {
        //    ResetCounter();
        //}
    }

    string FloatToText( float value )
    {
        return value.ToString( "f" + Mathf.Clamp( nbDecimal, 0, 10 ) );
    }

    //void DoMyWindow( int windowID )
    //{
    //    GUI.Label(
    //        new Rect( 0, 0, startRect.width, startRect.height ),
    //        FloatToText( CurrentFps ) + " FPS\n"
    //        + "avg " + FloatToText( AvarageFps )
    //        + FloatToText( LowestFps ) + "-" + FloatToText( HighestFps ),
    //        style
    //    );

    //    //if ( allowDrag ) {
    //    //    GUI.DragWindow( new Rect( 0, 0, Screen.width, Screen.height ) );
    //    //}
    //}
}