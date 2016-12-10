using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System
{
  /// <summary>
  /// Temporary class that holds static string variables, pointing to the Content folder in the Editor.
  /// TODO: We will need to use a URI-locator type of class that indicates if we're in Debug or not. The
  ///       paths will change depending on the chosen Configuration (and is better practice anyways).
  /// </summary>
  public class DefaultPaths
  {
    public const string ContentPath = "../../../Revolver/Editor/Debug/Content/";
    public const string XmlPath = "../../../Revolver/Editor/Debug/Content/XML/";
    public const string XmlTempPath = "../../../Revolver/Editor/Debug/Content/XML/Temp/";
    public const string ModelManagerPath = XmlPath + ModelManager;
    public const string DefaultPlayerPath = XmlPath + DefaultPlayer;
    public const string DefaultMapPath = XmlPath + DefaultMap;
    public const string ScreenManager3DPath = XmlPath + ScreenManager3D;
    public const string CameraManagerPath = XmlPath + CameraManager;
    public const string ProjectionCameraPath = XmlPath + ProjectionCamera;
    public const string DefaultPlayer = "Player.xml";
    public const string DefaultMap = "Minish Test.xml";
    public const string ContentFolder = "Content/";
    public const string XmlFolder = "Content/XML/";
    public const string ModelManager = "ModelManager.xml";
    public const string ScreenManager3D = "ScreenManager3D.xml";
    public const string CameraManager = "CameraManager.xml";
    public const string ProjectionCamera = "ProjectionCamera.xml";
    public const string GridCellPath = "Tools/Grid/GridCell";
  }
}
