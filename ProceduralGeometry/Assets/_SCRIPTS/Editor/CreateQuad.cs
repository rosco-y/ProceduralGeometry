/// EDITOR CLASS TO CREATE MESH WITH SPECIFIED ANCHOR
/// //////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.Collections;


// anchor point for created quad.
public enum AnchorPoint
{
        TopLeft,
        TopMiddle,
        TopRight,
        RightMiddle,
        BottomRight,
        BottomMiddle,
        BottomLeft,
        LeftMiddle,
        Center,
        Custom
}

public class CreateQuad : ScriptableWizard
{

        #region WIZARD VARIABLES
        public string MeshName = "Quad";
        public string GameObjectName = "Plane_Object";
        public string AssetsFolder = "Assets\\Meshes";
        // width and height of quad in world units.
        public float Width = 1.0f;
        public float Height = 1.0f;
        public AnchorPoint anchor = AnchorPoint.Center;
        public float AnchorX = 0.5f;
        public float AnchorY = 0.5f;
        #endregion

        #region WIZARD PREPARATIONS
        [MenuItem("GameObject/Create Other/Custom Plane")]
        static void CreateWizard() => _ = DisplayWizard("Create Plane", typeof(CreateQuad));


        // called when gui is shown
        //private void OnEnable() => GetFolderSelection();

        // Gets folder containing the currently selected asset in the project panel
        void GetFolderSelection()
        {
                if (Selection.objects != null && Selection.objects.Length == 1)
                {
                        AssetsFolder = AssetDatabase.GetAssetPath(Selection.objects[0]);
                        Debug.Log(AssetsFolder);
                }
        }


        //Called 10 times per second.
        private void OnInspectorUpdate()
        {
                switch (anchor)
                {
                        case AnchorPoint.TopLeft:
                                AnchorX = 0.0f * Width;
                                AnchorY = 1.0f * Height;
                                break;
                        case AnchorPoint.TopMiddle:
                                AnchorX = 0.5f * Width;
                                AnchorY = 1.0f * Height;
                                break;
                        case AnchorPoint.TopRight:
                                AnchorX = 1f * Width;
                                AnchorY = 1f * Height;
                                break;
                        case AnchorPoint.RightMiddle:
                                AnchorX = 1f * Width;
                                AnchorY = .5f * Height;
                                break;
                        case AnchorPoint.BottomRight:
                                AnchorX = 1f * Width;
                                AnchorY = 0;
                                break;
                        case AnchorPoint.BottomMiddle:
                                AnchorX = 0.5f * Width;
                                AnchorY = 0;
                                break;
                        case AnchorPoint.BottomLeft:
                                AnchorX = 0;
                                AnchorY = 0;
                                break;
                        case AnchorPoint.LeftMiddle:
                                AnchorX = 0;
                                AnchorY = .5f * Height;
                                break;
                        case AnchorPoint.Center:
                                AnchorX = 0.5f * Width;
                                AnchorY = 0.5f * Height;
                                break;
                        case AnchorPoint.Custom:
                        default:
                                break;

                }
        }
        #endregion

        #region WIZARD RESULTS
        // function to create quad mesh
        private void OnWizardCreate()
        {
                #region CREATE VERTICES AND FACES FOR THE QUAD MESH
                //Create the vertices and faces for the Quad Mesh
                Vector3[] Vertices = new Vector3[4];
                Vector2[] UVs = new Vector2[4];
                int[] Triangles = new int[6];

                //Assign vertices based on pivot
                //Bottom-left
                Vertices[0].x = -AnchorX;
                Vertices[0].y = -AnchorY;

                // bottom-right
                Vertices[1].x = Vertices[0].x + Width;
                Vertices[1].y = Vertices[0].y;

                // top-left
                Vertices[2].x = Vertices[0].x;
                Vertices[2].y = Vertices[0].y + Height;

                // top-right
                Vertices[3].x = Vertices[0].x + Width;
                Vertices[3].y = Vertices[0].y + Height;
                #endregion

                #region ASSIGN UVs
                // Assign UVs
                // Bottom-left
                UVs[0].x = 0;
                UVs[0].y = 0;
                // bottom-right
                UVs[1].x = 1f;
                UVs[1].y = 0;
                // top-left
                UVs[2].x = 0;
                UVs[2].y = 1f;
                //top-right
                UVs[3].x = 1f;
                UVs[3].y = 1f;
                #endregion

                #region ASSIGN TRIANGLES
                Triangles[0] = 3;
                Triangles[1] = 1;
                Triangles[2] = 2;

                Triangles[3] = 2;
                Triangles[4] = 1;
                Triangles[5] = 0;
                #endregion

                #region GENERATE MESH
                Mesh mesh = new Mesh
                {
                        name = MeshName,
                        vertices = Vertices,
                        uv = UVs,
                        triangles = Triangles
                };
                mesh.RecalculateNormals();
                #endregion

                #region ADD QUAD MESH ASSET TO AssetDatabase
                AssetDatabase.CreateAsset(mesh, AssetDatabase.GenerateUniqueAssetPath(AssetsFolder + "/" + MeshName)
                        + ".asset");
                AssetDatabase.SaveAssets();
                #endregion

                #region ADD QUAD MESH ASSET TO PROJECT
                // CREATE GAME OBJECT
                GameObject plane = new GameObject(GameObjectName);
                plane.name = MeshName;
                MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
                plane.AddComponent(typeof(MeshRenderer));

                //Assign mesh to mesh filter
                meshFilter.sharedMesh = mesh;
                mesh.RecalculateBounds();
                plane.AddComponent(typeof(BoxCollider));
                #endregion


        }
        #endregion
}


