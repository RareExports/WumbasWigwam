// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.World
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using NLog;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tao.OpenGl;

namespace WumbasWigwam
{
  internal class World
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private List<ObjectData[]> objectsStack = new List<ObjectData[]>();
    private List<ObjectData[]> structsStack = new List<ObjectData[]>();
    private List<CameraObject[]> camerasStack = new List<CameraObject[]>();
    private List<BKPath[]> pathsStack = new List<BKPath[]>();
    private List<string> modeStack = new List<string>();
    private List<int[]> selectedObjectsStack = new List<int[]>();
    private List<int[]> selectedStructsStack = new List<int[]>();
    private List<int[]> selectedNodesStack = new List<int[]>();
    private List<int[]> selectedSNodesStack = new List<int[]>();
    private List<int> selectedCamStack = new List<int>();
    private List<int> selectedPathStack = new List<int>();
    private int setupStackPointer;
    public int setupStackSize = 10;
    public float drawDistance = 5000f;
    public bool drawCams = true;
    public bool drawObjs = true;
    public bool drawA = true;
    public bool drawB = true;
    public bool pathMode;
    public PathMode pmode = PathMode.None;
    private List<int> selectedNodes = new List<int>();
    private List<int> selectedSNodes = new List<int>();
    public bool drawLevelBoundary;
    public bool drawLevelBoundaryAlpha;
    private float[][] frustum = new float[6][];
    private short oldX;
    private short oldY;
    private short oldZ;
    private short oldS;
    private short oldR;
    private ushort oldRad;
    private string path = Application.StartupPath;
    public List<byte> camBytes = new List<byte>();
    private SetupFileReader setupFileReader = new SetupFileReader();
    public List<ObjectData> objects = new List<ObjectData>();
    public List<ObjectData> structs = new List<ObjectData>();
    public List<CameraObject> cameras = new List<CameraObject>();
    public List<BKPath> paths = new List<BKPath>();
    public uint levelBoundary;
    public uint levelBoundaryAlpha;
    public uint levelDList;
    public uint levelBDList;
    public uint pickPathDList = 110000;
    public uint pickPathNodeDList = 110000;
    public uint jiggyDList = 200000;
    public uint clankerDL = 100000;
    public uint soundMapDList;
    public uint selectedDList = 200000;
    private List<short> UIDs = new List<short>();
    private List<byte> PathIDs = new List<byte>();
    public List<uint> objectsDList = new List<uint>();
    public List<uint> structsDList = new List<uint>();
    public List<uint> objectsPickingDList = new List<uint>();
    public List<uint> structsPickingDList = new List<uint>();
    public List<uint> pathsDList = new List<uint>();
    public List<uint> cameraDList = new List<uint>();
    public List<uint> cameraPickDList = new List<uint>();
    public uint movementDL = 100001;
    public SetupFile file;
    public string dir = "";
    public float nRange = 1000f;
    public List<int> selectedObjects = new List<int>();
    public List<int> selectedStructs = new List<int>();
    public int selectedCam = -1;
    public int selectedPath = -1;
    private Hashtable GLLevelObjects = new Hashtable();
    private Hashtable MMLevelObjects = new Hashtable();
    private Hashtable TTCLevelObjects = new Hashtable();
    private Hashtable CCLevelObjects = new Hashtable();
    private Hashtable BGSLevelObjects = new Hashtable();
    private Hashtable FPLevelObjects = new Hashtable();
    private Hashtable MMMLevelObjects = new Hashtable();
    private Hashtable GVLevelObjects = new Hashtable();
    private Hashtable CCWLevelObjects = new Hashtable();
    private Hashtable RBBLevelObjects = new Hashtable();
    private Hashtable SMLevelObjects = new Hashtable();
    private Hashtable FBLevelObjects = new Hashtable();
    private Hashtable CSLevelObjects = new Hashtable();
    public List<int> usedJiggyFlags = new List<int>();
    public List<int> usedHCFlags = new List<int>();
    public List<int> usedMTFlags = new List<int>();
    public bool[] jinjos = new bool[5];

    private void reloadStack()
    {
      this.DeleteAllStructs();
      this.DeleteAllObjects();
      this.DeleteAllCameras();
      this.EraseDLs();
      this.ResetPick();
      this.objects = ((IEnumerable<ObjectData>) this.objectsStack[this.setupStackPointer]).ToList<ObjectData>();
      this.structs = ((IEnumerable<ObjectData>) this.structsStack[this.setupStackPointer]).ToList<ObjectData>();
      this.cameras = ((IEnumerable<CameraObject>) this.camerasStack[this.setupStackPointer]).ToList<CameraObject>();
      this.paths = ((IEnumerable<BKPath>) this.pathsStack[this.setupStackPointer]).ToList<BKPath>();
      this.RecalculateUIDs();
    }

    public void pushSetupStack(string mode)
    {
      if (this.setupStackPointer != 0)
      {
        this.objectsStack.RemoveRange(0, this.setupStackPointer);
        this.structsStack.RemoveRange(0, this.setupStackPointer);
        this.camerasStack.RemoveRange(0, this.setupStackPointer);
        this.modeStack.RemoveRange(0, this.setupStackPointer);
        this.selectedObjectsStack.RemoveRange(0, this.setupStackPointer);
        this.selectedStructsStack.RemoveRange(0, this.setupStackPointer);
        this.selectedNodesStack.RemoveRange(0, this.setupStackPointer);
        this.selectedSNodesStack.RemoveRange(0, this.setupStackPointer);
        this.selectedCamStack.RemoveRange(0, this.setupStackPointer);
        this.selectedPathStack.RemoveRange(0, this.setupStackPointer);
        this.setupStackPointer = 0;
      }
      ObjectData[] objectDataArray1 = new ObjectData[this.objects.Count<ObjectData>()];
      for (int index = 0; index < objectDataArray1.Length; ++index)
        objectDataArray1[index] = ObjectData.fullClone(this.objects[index]);
      ObjectData[] objectDataArray2 = new ObjectData[this.structs.Count<ObjectData>()];
      for (int index = 0; index < objectDataArray2.Length; ++index)
        objectDataArray2[index] = ObjectData.fullClone(this.structs[index]);
      CameraObject[] cameraObjectArray = new CameraObject[this.cameras.Count<CameraObject>()];
      for (int index = 0; index < cameraObjectArray.Length; ++index)
        cameraObjectArray[index] = CameraObject.clone(this.cameras[index]);
      BKPath[] bkPathArray = new BKPath[this.paths.Count<BKPath>()];
      for (int index = 0; index < bkPathArray.Length; ++index)
        bkPathArray[index] = BKPath.clone(this.paths[index]);
      this.objectsStack.Insert(0, objectDataArray1);
      this.structsStack.Insert(0, objectDataArray2);
      this.camerasStack.Insert(0, cameraObjectArray);
      this.pathsStack.Insert(0, bkPathArray);
      this.modeStack.Insert(0, mode);
      this.selectedObjectsStack.Insert(0, this.selectedObjects.ToArray());
      this.selectedStructsStack.Insert(0, this.selectedStructs.ToArray());
      this.selectedNodesStack.Insert(0, this.selectedNodes.ToArray());
      this.selectedSNodesStack.Insert(0, this.selectedSNodes.ToArray());
      this.selectedCamStack.Insert(0, this.selectedCam);
      this.selectedPathStack.Insert(0, this.selectedPath);
      if (this.objectsStack.Count <= this.setupStackSize)
        return;
      this.objectsStack.RemoveAt(this.objectsStack.Count - 1);
      this.structsStack.RemoveAt(this.structsStack.Count - 1);
      this.camerasStack.RemoveAt(this.camerasStack.Count - 1);
      this.modeStack.RemoveAt(this.modeStack.Count - 1);
      this.selectedObjectsStack.RemoveAt(this.selectedObjectsStack.Count - 1);
      this.selectedStructsStack.RemoveAt(this.selectedStructsStack.Count - 1);
      this.selectedNodesStack.RemoveAt(this.selectedNodesStack.Count - 1);
      this.selectedSNodesStack.RemoveAt(this.selectedSNodesStack.Count - 1);
      this.selectedCamStack.RemoveAt(this.selectedCamStack.Count - 1);
      this.selectedPathStack.RemoveAt(this.selectedPathStack.Count - 1);
    }

    public bool popSetupStack()
    {
      if (this.modeStack.Count<string>() > this.setupStackPointer + 1)
      {
        ++this.setupStackPointer;
        this.reloadStack();
        return true;
      }
      int num = (int) MessageBox.Show("No more moves to undo on stack");
      return false;
    }

    public bool forwardSetupStack(int sp)
    {
      if (this.setupStackPointer - sp >= 0)
      {
        this.setupStackPointer -= sp;
        this.reloadStack();
        return true;
      }
      int num = (int) MessageBox.Show("No more moves to redo on stack");
      return false;
    }

    public bool setStackLocation(int loc)
    {
      if (loc > this.objectsStack.Count - 1 || loc < 0)
        return false;
      this.setupStackPointer = loc;
      this.reloadStack();
      return true;
    }

    public void resetStack()
    {
      this.objectsStack.Clear();
      this.structsStack.Clear();
      this.camerasStack.Clear();
      this.pathsStack.Clear();
      this.modeStack.Clear();
      this.selectedObjectsStack.Clear();
      this.selectedStructsStack.Clear();
      this.selectedNodesStack.Clear();
      this.selectedSNodesStack.Clear();
      this.selectedCamStack.Clear();
      this.selectedPathStack.Clear();
      this.setupStackPointer = 0;
    }

    public List<string> getHistoryStack() => this.modeStack;

    public void DrawLevelBoundary()
    {
      if (this.file == null)
        return;
      this.levelBoundary = (uint) GL.GenLists(1);
      GL.NewList(this.levelBoundary, ListMode.Compile);
      if (this.file.modelAPointer != 0)
        Core.DrawModelBoundary(this.file.bounds, true);
      GL.EndList();
      this.levelBoundaryAlpha = (uint) GL.GenLists(1);
      GL.NewList(this.levelBoundaryAlpha, ListMode.Compile);
      if (this.file.modelBPointer != 0)
        Core.DrawModelBoundary(this.file.boundsAlphaModel, true);
      GL.EndList();
    }

    public void ActivatePathMode()
    {
      this.pathMode = true;
      this.pmode = PathMode.None;
      this.RenderPaths();
    }

    public void DeactivatePathMode()
    {
      this.pathMode = false;
      this.pmode = PathMode.None;
    }

    public bool CreateNewPath(ObjectData node)
    {
      short newUid = this.GetNewUID();
      if (newUid == (short) 0)
        return false;
      node.obj16 = (byte) ((uint) newUid >> 8);
      node.obj17 = (byte) newUid;
      node.obj18 = (byte) 0;
      node.uid = newUid;
      this.paths.Add(new BKPath() { nodes = { node } });
      this.RenderPaths();
      this.renderPathPicking();
      this.selectedPath = this.paths.Count - 1;
      return true;
    }

    public short GetNewUID()
    {
      this.UIDs.Sort();
      short num1 = 768;
      if (this.UIDs.Count == 0)
      {
        short num2 = 768;
        this.UIDs.Add(num2);
        return num2;
      }
      int num3;
      for (int index = 0; index < this.UIDs.Count; index = num3 + 1)
      {
        if (num1 > (short) 4080)
          return 0;
        if (this.UIDs.Contains(num1))
        {
          num3 = this.UIDs.IndexOf(num1);
          num1 += (short) 16;
        }
        else
          break;
      }
      this.UIDs.Add(num1);
      return num1;
    }

    private byte GetNewPathID()
    {
      for (int index = 0; index < this.paths[this.selectedPath].nodes.Count; ++index)
      {
        if (this.paths[this.selectedPath].nodes[index].type == ObjectType.SPath)
          return this.paths[this.selectedPath].nodes[index].pathID;
      }
      this.PathIDs.Sort();
      byte num1 = 1;
      if (this.PathIDs.Count == 0)
      {
        this.PathIDs.Add(num1);
        return num1;
      }
      byte num2;
      for (num2 = (byte) 1; (int) num2 < this.PathIDs.Count; ++num2)
      {
        if (num2 > (byte) 254)
          return 0;
        if (!this.PathIDs.Contains(num2))
          break;
      }
      this.PathIDs.Add(num2);
      return num2;
    }

    public bool AddNode(ObjectData node)
    {
      short newUid = this.GetNewUID();
      if (newUid == (short) 0)
        return false;
      node.obj16 = (byte) ((uint) newUid >> 8);
      node.obj17 = (byte) newUid;
      try
      {
        if (node.type == ObjectType.SPath)
        {
          byte newPathId = this.GetNewPathID();
          node.setPathID(newPathId);
          node.locX = this.paths[this.selectedPath].nodes[0].locX;
          node.locY = this.paths[this.selectedPath].nodes[0].locY;
          node.locZ = this.paths[this.selectedPath].nodes[0].locZ;
          node.nodeOutUID = this.paths[this.selectedPath].nodes[0].nodeOutUID;
          node.obj18 = this.paths[this.selectedPath].nodes[0].obj18;
          this.paths[this.selectedPath].nodes[0].obj18 = (byte) ((uint) newUid / 16U);
          this.paths[this.selectedPath].nodes[0].nodeOutUID = newUid;
          this.paths[this.selectedPath].nodes[0].node_out = true;
          node.node_in = true;
          node.node_out = true;
        }
        else
        {
          for (int index = this.paths[this.selectedPath].nodes.Count - 1; index > -1; ++index)
          {
            if (this.paths[this.selectedPath].nodes[index].type == ObjectType.Path)
            {
              this.paths[this.selectedPath].nodes[index].obj18 = (byte) ((uint) newUid / 16U);
              this.paths[this.selectedPath].nodes[index].nodeOutUID = newUid;
              this.paths[this.selectedPath].nodes[index].node_out = true;
              node.node_in = true;
              break;
            }
          }
        }
      }
      catch
      {
      }
      node.uid = newUid;
      this.paths[this.selectedPath].nodes.Add(node);
      this.redrawSelectedPath();
      return true;
    }

    public void AddSNode()
    {
      ObjectData objectData = new ObjectData(0.0f, 0, 0, 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0, true, 0);
      short newUid = this.GetNewUID();
      objectData.obj16 = (byte) ((uint) newUid >> 8);
      objectData.obj17 = (byte) newUid;
      objectData.uid = newUid;
      this.paths[this.selectedPath].nodes.Add(objectData);
      this.redrawSelectedPath();
    }

    public void UpdateSNode(
      short to,
      float actPercent,
      int w1,
      bool usePause,
      bool useSpeed,
      bool useAnimation,
      byte pathID,
      int speed,
      int pauseTime,
      int unk3)
    {
      for (int index = 0; index < this.selectedSNodes.Count; ++index)
      {
        foreach (ObjectData node in this.paths[this.selectedPath].nodes)
        {
          if ((int) node.nodeOutUID == (int) to)
          {
            node.nodeOutUID = (short) 0;
            node.obj18 = (byte) 0;
            node.node_out = false;
          }
        }
        int selectedSnode = this.selectedSNodes[index];
        if (to != (short) 0)
          this.paths[this.selectedPath].nodes[selectedSnode].node_out = true;
        this.paths[this.selectedPath].nodes[selectedSnode].nodeOutUID = to;
        this.paths[this.selectedPath].nodes[selectedSnode].obj18 = (byte) ((uint) to / 16U);
        this.paths[this.selectedPath].nodes[selectedSnode].activationPercent = actPercent;
        this.paths[this.selectedPath].nodes[selectedSnode].pw1 = w1;
        this.paths[this.selectedPath].nodes[selectedSnode].setPathID(pathID);
        this.paths[this.selectedPath].nodes[selectedSnode].setControlFlags(usePause, useSpeed, useAnimation);
        this.paths[this.selectedPath].nodes[selectedSnode].setSpeed(speed);
        this.paths[this.selectedPath].nodes[selectedSnode].setPauseTime(pauseTime);
        this.paths[this.selectedPath].nodes[selectedSnode].setUNK3(unk3);
      }
      this.redrawSelectedPath();
    }

    public void UpdateNodeLocation(
      short x,
      short y,
      short z,
      bool ux,
      bool uy,
      bool uz,
      short yOffset)
    {
      try
      {
        short[] forSelectedObjects = this.GetCenterPointForSelectedObjects(x, y, z, Mode.Path);
        for (int index = 0; index < this.selectedSNodes.Count; ++index)
        {
          int selectedSnode = this.selectedSNodes[index];
          if (ux)
            this.paths[this.selectedPath].nodes[selectedSnode].locX += forSelectedObjects[0];
          if (uy)
            this.paths[this.selectedPath].nodes[selectedSnode].locY += (short) ((int) forSelectedObjects[1] + (int) yOffset);
          if (uz)
            this.paths[this.selectedPath].nodes[selectedSnode].locZ += forSelectedObjects[2];
        }
        for (int index = 0; index < this.selectedNodes.Count; ++index)
        {
          int selectedNode = this.selectedNodes[index];
          if (ux)
            this.paths[this.selectedPath].nodes[selectedNode].locX += forSelectedObjects[0];
          if (uy)
            this.paths[this.selectedPath].nodes[selectedNode].locY += (short) ((int) forSelectedObjects[1] + (int) yOffset);
          if (uz)
            this.paths[this.selectedPath].nodes[selectedNode].locZ += forSelectedObjects[2];
        }
      }
      catch
      {
      }
    }

    public void UpdateNode(short to, short x, short y, short z)
    {
      try
      {
        int selectedNodeIndex = this.GetSelectedNodeIndex();
        if (selectedNodeIndex == -1)
          return;
        foreach (ObjectData node in this.paths[this.selectedPath].nodes)
        {
          if ((int) node.nodeOutUID == (int) to)
          {
            node.nodeOutUID = (short) 0;
            node.obj18 = (byte) 0;
            node.node_out = false;
          }
        }
        this.paths[this.selectedPath].nodes[selectedNodeIndex].nodeOutUID = to;
        this.paths[this.selectedPath].nodes[selectedNodeIndex].obj18 = (byte) ((uint) to / 16U);
        if (to != (short) 0)
          this.paths[this.selectedPath].nodes[selectedNodeIndex].node_out = true;
        this.paths[this.selectedPath].nodes[selectedNodeIndex].locX = x;
        this.paths[this.selectedPath].nodes[selectedNodeIndex].locY = y;
        this.paths[this.selectedPath].nodes[selectedNodeIndex].locZ = z;
      }
      catch
      {
      }
    }

    public ObjectType GetSelectedNodeType()
    {
      if (this.selectedPath == -1)
        return ObjectType.Path;
      if (this.selectedSNodes.Count<int>() == 1)
        return ObjectType.SPath;
      this.selectedNodes.Count<int>();
      return ObjectType.Path;
    }

    public int GetSelectedNodeIndex()
    {
      int num = -1;
      if (this.selectedNodes.Count<int>() == 1)
        num = this.selectedNodes[0];
      if (this.selectedSNodes.Count<int>() == 1)
        num = this.selectedSNodes[0];
      return num;
    }

    public bool HasSelectedCamera() => this.selectedCam != -1;

    public CameraObject GetSelectedCamera() => this.selectedCam != -1 ? this.cameras[this.selectedCam] : throw new Exception("No camera selected");

    public bool HasSelectedNodes() => this.selectedNodes.Count > 0 || this.selectedSNodes.Count > 0;

    public bool SingleNodeSelected()
    {
      if (this.selectedNodes.Count == 1 && this.selectedSNodes.Count == 0)
        return true;
      return this.selectedNodes.Count == 0 && this.selectedSNodes.Count == 1;
    }

    public void SetSelectNode(int id)
    {
      this.selectedSNodes.Clear();
      this.selectedNodes.Clear();
      if (this.selectedPath == -1 || this.paths[this.selectedPath].nodes.Count <= id)
        return;
      if (this.paths[this.selectedPath].nodes[id].type == ObjectType.SPath)
        this.selectedSNodes.Add(id);
      else
        this.selectedNodes.Add(id);
    }

    private void RecalculateUIDs()
    {
      this.UIDs.Clear();
      this.PathIDs.Clear();
      for (int index = 0; index < this.objects.Count; ++index)
        this.UIDs.Add(this.objects[index].uid);
      for (int index1 = 0; index1 < this.paths.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.paths[index1].nodes.Count; ++index2)
        {
          this.UIDs.Add(this.paths[index1].nodes[index2].uid);
          if (!this.PathIDs.Contains(this.paths[index1].nodes[index2].pathID))
            this.PathIDs.Add(this.paths[index1].nodes[index2].pathID);
        }
      }
    }

    private void ClearNodeIn(ObjectData node)
    {
      if (this.selectedPath == -1)
        return;
      foreach (ObjectData node1 in this.paths[this.selectedPath].nodes)
      {
        if ((int) node.nodeOutUID == (int) node1.uid)
          node1.node_in = false;
        else if ((int) node.uid == (int) node1.nodeOutUID)
        {
          node1.nodeOutUID = (short) 0;
          node1.node_out = false;
        }
      }
    }

    public void RemoveNode()
    {
      if (this.selectedPath == -1)
        return;
      if (this.selectedNodes.Count > 0)
      {
        this.selectedNodes.Sort();
        this.selectedNodes.Reverse();
        for (int index = 0; index < this.selectedNodes.Count; ++index)
        {
          this.ClearNodeIn(this.paths[this.selectedPath].nodes[this.selectedNodes[index]]);
          this.paths[this.selectedPath].nodes.RemoveAt(this.selectedNodes[index]);
        }
      }
      if (this.selectedSNodes.Count > 0)
      {
        for (int index = 0; index < this.selectedSNodes.Count; ++index)
        {
          this.ClearNodeIn(this.paths[this.selectedPath].nodes[this.selectedSNodes[index]]);
          this.paths[this.selectedPath].nodes.RemoveAt(this.selectedSNodes[index]);
        }
      }
      GL.DeleteLists(this.pathsDList[this.selectedPath], 1);
      uint list = (uint) GL.GenLists(1);
      GL.NewList(list, ListMode.Compile);
      this.DrawPath(this.paths[this.selectedPath], false);
      GL.EndList();
      this.pathsDList[this.selectedPath] = list;
      this.RecalculateUIDs();
      this.ClearSelectedNodes();
      this.renderPathPicking();
    }

    public void RemovePath()
    {
      if (this.selectedPath == -1)
        return;
      this.paths.RemoveAt(this.selectedPath);
      GL.DeleteLists(this.pathsDList[this.selectedPath], 1);
      this.RecalculateUIDs();
      this.ResetPick();
      this.renderPathPicking();
      this.RenderPaths();
    }

    public void SetAnimationOnNode(int offset)
    {
      if (this.selectedSNodes.Count != 1)
        return;
      this.paths[this.selectedPath].nodes[this.selectedSNodes[0]].pw2 = (offset << 5 << 16) + 5895;
      this.paths[this.selectedPath].nodes[this.selectedSNodes[0]].animation = offset;
    }

    private void CreateJiggyDL() => this.jiggyDList = Core.DrawObject(new ObjectData((short) 70, 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 6412));

    private void checkSize(ref float s, ObjectData obj)
    {
      if (obj.Pointer != 0 && obj.Pointer != 1)
        return;
      s = 1f;
    }

    public void ClearSelectedNodes()
    {
      this.selectedNodes.Clear();
      this.selectedSNodes.Clear();
    }

    public void ResetPick()
    {
      this.selectedObjects.Clear();
      this.selectedStructs.Clear();
      this.selectedPath = -1;
      this.selectedNodes.Clear();
      this.selectedSNodes.Clear();
      this.selectedCam = -1;
    }

    public int GetCamType() => this.selectedCam == -1 ? -1 : this.cameras[this.selectedCam].type;

    public void BKCameraToBBCamera(GLCamera BBCam)
    {
      if (this.selectedCam == -1)
        return;
      CameraObject camera = this.cameras[this.selectedCam];
      camera.x = -BBCam.X;
      camera.y = -BBCam.Y;
      camera.z = -BBCam.Z;
      camera.yaw = -(double) BBCam.GetYRotation() < 0.0 ? (float) (360.0 + -(double) BBCam.GetYRotation()) : -BBCam.GetYRotation();
      camera.pitch = -(double) BBCam.GetXRotation() < 0.0 ? (float) (360.0 + -(double) BBCam.GetXRotation()) : -BBCam.GetXRotation();
      camera.roll = 0.0f;
    }

    public void BBCameraToBKCamera(GLCamera BBCam)
    {
      if (this.selectedCam == -1)
        return;
      CameraObject camera = this.cameras[this.selectedCam];
      BBCam.X = -camera.x;
      BBCam.Y = -camera.y;
      BBCam.Z = -camera.z;
      BBCam.Yaw = -camera.yaw;
      BBCam.Pitch = -camera.pitch;
      BBCam.Roll = 0.0f;
    }

    public void updateCameraLocation(float x, float y, float z, bool ux, bool uy, bool uz)
    {
      if (this.selectedCam == -1)
        return;
      if (ux)
        this.cameras[this.selectedCam].x = x;
      if (uy)
        this.cameras[this.selectedCam].y = y;
      if (!uz)
        return;
      this.cameras[this.selectedCam].z = z;
    }

    public void updateCameraYaw(float yaw)
    {
      if (this.selectedCam == -1)
        return;
      this.cameras[this.selectedCam].yaw += (float) (short) yaw;
      if ((double) this.cameras[this.selectedCam].yaw < 0.0)
        this.cameras[this.selectedCam].yaw = 360f;
      if ((double) this.cameras[this.selectedCam].yaw <= 360.0)
        return;
      this.cameras[this.selectedCam].yaw = 0.0f;
    }

    public void updateCameraPitch(float pitch)
    {
      if (this.selectedCam == -1)
        return;
      this.cameras[this.selectedCam].pitch += (float) (short) pitch;
      if ((double) this.cameras[this.selectedCam].pitch < 0.0)
        this.cameras[this.selectedCam].pitch = 360f;
      if ((double) this.cameras[this.selectedCam].pitch <= 360.0)
        return;
      this.cameras[this.selectedCam].pitch = 0.0f;
    }

    public void updateCameraRoll(float roll)
    {
      if (this.selectedCam == -1)
        return;
      this.cameras[this.selectedCam].roll += (float) (short) roll;
      if ((double) this.cameras[this.selectedCam].roll < 0.0)
        this.cameras[this.selectedCam].roll = 360f;
      if ((double) this.cameras[this.selectedCam].roll <= 360.0)
        return;
      this.cameras[this.selectedCam].roll = 0.0f;
    }

    public void updateObject(
      short id,
      short ss,
      short flag,
      ushort radius,
      short x,
      short y,
      short z,
      short size,
      short rot,
      byte b10,
      byte b11,
      byte b13,
      byte b18,
      byte b16,
      byte b17)
    {
      ObjectData objectData = this.selectedObjects.Count > 0 ? this.objects[this.selectedObjects[0]] : this.structs[this.selectedStructs[0]];
      objectData.objectID = id;
      objectData.specialScript = ss;
      objectData.flag = (int) flag;
      objectData.radius = radius;
      objectData.locX = x;
      objectData.locY = y;
      objectData.locZ = z;
      objectData.size = size;
      objectData.rot = rot;
      objectData.obj10 = b10;
      objectData.obj11 = b11;
      objectData.obj13 = b13;
      objectData.obj18 = b18;
      objectData.obj16 = b16;
      objectData.obj17 = b17;
    }

    public void updateObjectID(short id)
    {
      if (!this.hasSelectedObjects())
        return;
      ObjectData o = this.selectedObjects.Count > 0 ? this.objects[this.selectedObjects[0]] : this.structs[this.selectedStructs[0]];
      if (o.type == ObjectType.Flag)
      {
        short num = 0;
        if (o.name.Contains("Jiggy"))
          num = (short) 70;
        if (o.name.Contains("Empty Honeycomb"))
          num = (short) 71;
        if (o.name.Contains("Mumbo Token"))
          num = (short) 45;
        if (num != (short) 0)
        {
          foreach (ObjectData objectData in this.objects)
          {
            if ((int) objectData.objectID == (int) num && objectData.flag == (int) o.objectID)
            {
              objectData.flag = (int) id;
              break;
            }
          }
        }
      }
      o.objectID = id;
      if (o.type != ObjectType.Normal)
        return;
      ObjectDB.FillObjectDetails(ref o);
    }

    public void updateObjectScript(short s)
    {
      if (!this.hasSelectedObjects())
        return;
      ObjectData o = this.selectedObjects.Count > 0 ? this.objects[this.selectedObjects[0]] : this.structs[this.selectedStructs[0]];
      o.specialScript = s;
      ObjectDB.FillObjectDetails(ref o);
    }

    public void updateCamera(
      short id,
      float x,
      float y,
      float z,
      float yaw,
      float pitch,
      float roll)
    {
      CameraObject camera = this.cameras[this.selectedCam];
      camera.id = id;
      camera.x = x;
      camera.y = y;
      camera.z = z;
      camera.yaw = yaw;
      camera.pitch = pitch;
      camera.roll = roll;
    }

    public void updateCamera3(
      short id,
      float x,
      float y,
      float z,
      float yaw,
      float pitch,
      float roll,
      float hs,
      float vs,
      float r,
      float accel,
      int a5,
      float cd,
      float fd)
    {
      CameraObject camera = this.cameras[this.selectedCam];
      camera.id = id;
      camera.x = x;
      camera.y = y;
      camera.z = z;
      camera.yaw3 = yaw;
      camera.pitch3 = pitch;
      camera.roll3 = roll;
      camera.Type3Arg5 = a5;
      camera.camCDist = cd;
      camera.camFDist = fd;
      camera.camHSpeed = hs;
      camera.camVSpeed = vs;
      camera.camRotation = r;
      camera.camAccel = accel;
    }

    public void updateCamera1(
      short id,
      float x,
      float y,
      float z,
      float yaw,
      float pitch,
      float roll,
      float hs,
      float vs,
      float r,
      float accel,
      int a5)
    {
      CameraObject camera = this.cameras[this.selectedCam];
      camera.id = id;
      camera.x = x;
      camera.y = y;
      camera.z = z;
      camera.yaw3 = yaw;
      camera.pitch3 = pitch;
      camera.roll3 = roll;
      camera.Type3Arg5 = a5;
      camera.camHSpeed = hs;
      camera.camVSpeed = vs;
      camera.camRotation = r;
      camera.camAccel = accel;
    }

    public bool isObject() => this.selectedObjects.Count > 0;

    public void setSelectedObj(int sid, int objType_)
    {
      if (objType_ == 0)
        this.selectedObjects.Add(sid);
      else
        this.selectedStructs.Add(sid);
    }

    public void setNodeAsEndNode()
    {
      if (this.selectedPath == -1)
        return;
      int index = -1;
      if (this.selectedSNodes.Count == 1)
        index = this.selectedSNodes[0];
      if (this.selectedNodes.Count == 1)
        index = this.selectedNodes[0];
      if (index == -1)
        return;
      this.paths[this.selectedPath].nodes[index].nodeOutUID = (short) 0;
      this.paths[this.selectedPath].nodes[index].obj18 = (byte) 0;
    }

    public bool hasSelectedObjects() => this.selectedObjects.Count > 0 || this.selectedStructs.Count > 0;

    public bool singleObjectSelected()
    {
      if (!this.hasSelectedObjects())
        return false;
      if (this.selectedObjects.Count == 1 && this.selectedStructs.Count == 0)
        return true;
      return this.selectedObjects.Count == 0 && this.selectedStructs.Count == 1;
    }

    public ObjectData getSelectedObject() => this.selectedObjects.Count <= 0 ? this.structs[this.selectedStructs[0]] : this.objects[this.selectedObjects[0]];

    public World()
    {
      for (int index = 0; index < this.frustum.Length; ++index)
        this.frustum[index] = new float[4];
    }

    private void PairObjectsWithFlags()
    {
      this.usedHCFlags.Clear();
      this.usedJiggyFlags.Clear();
      this.usedMTFlags.Clear();
      List<int> intList = new List<int>();
      foreach (ObjectData objectData in this.objects)
        objectData.flag = -1;
      using (List<ObjectData>.Enumerator enumerator = this.objects.GetEnumerator())
      {
label_17:
        while (enumerator.MoveNext())
        {
          ObjectData current = enumerator.Current;
          if (current.type == ObjectType.Flag)
          {
            bool flag = false;
            int index = 0;
            while (true)
            {
              if (index < this.objects.Count && !flag)
              {
                if ((this.objects[index].objectID == (short) 70 || this.objects[index].objectID == (short) 45 || this.objects[index].objectID == (short) 71) && !intList.Contains(index))
                {
                  ushort radius = current.radius;
                  if ((int) this.objects[index].locX < (int) current.locX + (int) radius && (int) this.objects[index].locX > (int) current.locX - (int) radius && (int) this.objects[index].locY < (int) current.locY + (int) radius && (int) this.objects[index].locY > (int) current.locY - (int) radius && (int) this.objects[index].locZ < (int) current.locZ + (int) radius && (int) this.objects[index].locZ > (int) current.locZ - (int) radius)
                  {
                    intList.Add(index);
                    this.objects[index].flag = (int) current.objectID;
                    flag = true;
                    switch (this.objects[index].objectID)
                    {
                      case 45:
                        this.usedMTFlags.Add((int) current.objectID);
                        current.name = "Mumbo Token Flag";
                        break;
                      case 70:
                        this.usedJiggyFlags.Add((int) current.objectID);
                        current.name = "Jiggy Flag";
                        break;
                      case 71:
                        this.usedHCFlags.Add((int) current.objectID);
                        current.name = "Empty Honeycomb Flag";
                        break;
                    }
                  }
                }
                ++index;
              }
              else
                goto label_17;
            }
          }
        }
      }
    }

    public string getCameraDescription(int cid)
    {
      string str = "";
      foreach (ObjectData objectData in this.objects)
      {
        if (objectData.cameraID == cid)
          str = objectData.name;
      }
      return str;
    }

    public void EraseDLs()
    {
      try
      {
        GL.DeleteLists(this.levelDList, 1);
      }
      catch (Exception ex)
      {
      }
      try
      {
        GL.DeleteLists(this.levelBDList, 1);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.objectsDList);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.structsDList);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.objectsPickingDList);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.structsPickingDList);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.cameraDList);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.cameraPickDList);
      }
      catch
      {
      }
      try
      {
        GL.DeleteLists(this.clankerDL, 1);
      }
      catch
      {
      }
      try
      {
        Core.DeleteDLs(this.pathsDList);
      }
      catch
      {
      }
      this.objectsDList.Clear();
      this.structsDList.Clear();
      this.objectsPickingDList.Clear();
      this.structsPickingDList.Clear();
      this.cameraDList.Clear();
      this.cameraPickDList.Clear();
      this.pathsDList.Clear();
    }

    public void addCamera(CameraObject cam)
    {
      this.cameras.Add(cam);
      uint list = (uint) GL.GenLists(1);
      GL.NewList(list, ListMode.Compile);
      Core.DrawCamera(cam);
      GL.EndList();
      this.cameraDList.Add(list);
      this.cameraPickDList.Add(Core.DrawCameraPicking(cam));
    }

    public void Redraw(GLCamera BBCam)
    {
      GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
      if (this.pathMode)
      {
        if (this.pmode == PathMode.Assign)
        {
          GL.PushMatrix();
          GL.CallList(this.levelDList);
          this.ExtractFrustum();
          for (int index = 0; index < this.objects.Count<ObjectData>(); ++index)
          {
            try
            {
              this.DrawObjectDisplayList(this.objects[index], this.objectsDList[index], BBCam);
            }
            catch (Exception ex)
            {
            }
          }
          GL.CallList(this.levelBDList);
          GL.PopMatrix();
        }
        else if (this.pmode == PathMode.Link)
        {
          GL.PushMatrix();
          GL.CallList(this.levelDList);
          if (this.selectedPath != -1)
          {
            GL.DeleteLists(this.pathsDList[this.selectedPath], 1);
            uint list = (uint) GL.GenLists(1);
            GL.NewList(list, ListMode.Compile);
            this.DrawPath(this.paths[this.selectedPath], false);
            GL.EndList();
            this.pathsDList[this.selectedPath] = list;
            GL.CallList(this.pathsDList[this.selectedPath]);
          }
          GL.CallList(this.levelBDList);
          GL.PopMatrix();
        }
        else
        {
          GL.PushMatrix();
          if (this.drawLevelBoundary)
            GL.CallList(this.levelBoundary);
          if (this.drawLevelBoundaryAlpha)
            GL.CallList(this.levelBoundaryAlpha);
          GL.CallList(this.levelDList);
          if (this.selectedPath != -1)
          {
            GL.DeleteLists(this.pathsDList[this.selectedPath], 1);
            uint list = (uint) GL.GenLists(1);
            GL.NewList(list, ListMode.Compile);
            this.DrawPath(this.paths[this.selectedPath], false);
            GL.EndList();
            this.pathsDList[this.selectedPath] = list;
            GL.CallList(this.pathsDList[this.selectedPath]);
          }
          else
          {
            for (int index = 0; index < this.pathsDList.Count<uint>(); ++index)
              GL.CallList(this.pathsDList[index]);
          }
          if (this.selectedPath != -1)
          {
            GL.DeleteLists(this.selectedDList, 1);
            this.selectedDList = (uint) GL.GenLists(1);
            GL.NewList(this.selectedDList, ListMode.Compile);
            for (int index = 0; index < this.selectedNodes.Count; ++index)
              Core.drawObjectBoundingBox(this.paths[this.selectedPath].nodes[this.selectedNodes[index]]);
            GL.EndList();
            GL.CallList(this.selectedDList);
            if (this.selectedSNodes.Count == 1)
              this.DrawSelectedPathController();
          }
          GL.CallList(this.levelBDList);
          GL.PopMatrix();
        }
      }
      else
      {
        GL.DeleteLists(this.selectedDList, 1);
        this.selectedDList = (uint) GL.GenLists(1);
        GL.NewList(this.selectedDList, ListMode.Compile);
        for (int index = 0; index < this.selectedObjects.Count<int>(); ++index)
          Core.drawObjectBoundingBox(this.objects[this.selectedObjects[index]]);
        for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
          Core.drawObjectBoundingBox(this.structs[this.selectedStructs[index]]);
        if (this.selectedCam != -1)
        {
          GL.PushMatrix();
          GL.Translate(this.cameras[this.selectedCam].x, this.cameras[this.selectedCam].y, this.cameras[this.selectedCam].z);
          GL.Rotate(this.cameras[this.selectedCam].roll, 0.0f, 0.0f, 1f);
          GL.Rotate(this.cameras[this.selectedCam].yaw, 0.0f, 1f, 0.0f);
          GL.Rotate(this.cameras[this.selectedCam].pitch, 1f, 0.0f, 0.0f);
          Core.DrawModelBoundary(this.cameras[this.selectedCam].bb, false);
          GL.PopMatrix();
        }
        GL.EndList();
        GL.PushMatrix();
        if (this.drawLevelBoundary)
          GL.CallList(this.levelBoundary);
        if (this.drawLevelBoundaryAlpha)
          GL.CallList(this.levelBoundaryAlpha);
        if (this.drawA)
          GL.CallList(this.levelDList);
        GL.CallList(this.selectedDList);
        if (this.drawObjs)
        {
          this.ExtractFrustum();
          for (int index = 0; index < this.objects.Count<ObjectData>(); ++index)
          {
            try
            {
              this.DrawObjectDisplayList(this.objects[index], this.objectsDList[index], BBCam);
            }
            catch (Exception ex)
            {
            }
          }
          for (int index = 0; index < this.structs.Count<ObjectData>(); ++index)
          {
            try
            {
              this.DrawObjectDisplayList(this.structs[index], this.structsDList[index], BBCam);
            }
            catch
            {
            }
          }
        }
        if (this.file != null && this.file.pointer == 38864)
          GL.CallList(this.clankerDL);
        if (this.drawB)
          GL.CallList(this.levelBDList);
        if (this.drawCams)
        {
          for (int index = 0; index < this.cameraDList.Count<uint>(); ++index)
          {
            CameraObject camera = this.cameras[index];
            GL.PushMatrix();
            if (this.cameras[index].type == 2)
            {
              GL.Translate(camera.x, camera.y, camera.z);
              GL.Rotate(camera.roll, 0.0f, 0.0f, 1f);
              GL.Rotate(camera.yaw, 0.0f, 1f, 0.0f);
              GL.Rotate(camera.pitch, 1f, 0.0f, 0.0f);
            }
            if (camera.type == 3 || camera.type == 1)
              GL.Translate(camera.x, camera.y, camera.z);
            GL.CallList(this.cameraDList[index]);
            GL.PopMatrix();
          }
        }
        GL.PopMatrix();
      }
    }

    private void SetupObjectMatrix(ObjectData obj)
    {
      GL.Translate((float) obj.locX, (float) obj.locY, (float) obj.locZ);
      if (obj.Pointer == 32808 || obj.Pointer == 34432)
        GL.Rotate((float) obj.rot, 0.0f, 0.0f, 1f);
      else if (obj.Pointer != 32872)
        GL.Rotate((float) obj.rot, 0.0f, 1f, 0.0f);
      float s = (float) obj.size / 100f;
      this.checkSize(ref s, obj);
      GL.Scale(s, s, s);
    }

    private void DrawObjectDisplayList(ObjectData obj, uint dl, GLCamera cam)
    {
      float s = (float) obj.size / 100f;
      this.checkSize(ref s, obj);
      if (!this.ObjectInDrawDistance((float) obj.locX, (float) obj.locY, (float) obj.locZ, cam) || !this.CubeInFrustum((float) obj.locX, (float) obj.locY, (float) obj.locZ, (float) obj.bb.getSize() * s))
        return;
      GL.PushMatrix();
      this.SetupObjectMatrix(obj);
      GL.CallList(dl);
      GL.PopMatrix();
    }

    private bool ObjectInDrawDistance(float x, float y, float z, GLCamera camera) => Math.Sqrt(Math.Pow((double) x + (double) camera.X, 2.0) + Math.Pow((double) y + (double) camera.Y, 2.0) + Math.Pow((double) z + (double) camera.Z, 2.0)) <= (double) this.drawDistance;

    private bool CubeInFrustum(float x, float y, float z, float size)
    {
      for (int index = 0; index < 6; ++index)
      {
        if ((double) this.frustum[index][0] * ((double) x - (double) size) + (double) this.frustum[index][1] * ((double) y - (double) size) + (double) this.frustum[index][2] * ((double) z - (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x + (double) size) + (double) this.frustum[index][1] * ((double) y - (double) size) + (double) this.frustum[index][2] * ((double) z - (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x - (double) size) + (double) this.frustum[index][1] * ((double) y + (double) size) + (double) this.frustum[index][2] * ((double) z - (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x + (double) size) + (double) this.frustum[index][1] * ((double) y + (double) size) + (double) this.frustum[index][2] * ((double) z - (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x - (double) size) + (double) this.frustum[index][1] * ((double) y - (double) size) + (double) this.frustum[index][2] * ((double) z + (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x + (double) size) + (double) this.frustum[index][1] * ((double) y - (double) size) + (double) this.frustum[index][2] * ((double) z + (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x - (double) size) + (double) this.frustum[index][1] * ((double) y + (double) size) + (double) this.frustum[index][2] * ((double) z + (double) size) + (double) this.frustum[index][3] <= 0.0 && (double) this.frustum[index][0] * ((double) x + (double) size) + (double) this.frustum[index][1] * ((double) y + (double) size) + (double) this.frustum[index][2] * ((double) z + (double) size) + (double) this.frustum[index][3] <= 0.0)
          return false;
      }
      return true;
    }

    private bool PointInFrustum(float x, float y, float z)
    {
      for (int index = 0; index < 6; ++index)
      {
        if ((double) this.frustum[index][0] * (double) x + (double) this.frustum[index][1] * (double) y + (double) this.frustum[index][2] * (double) z + (double) this.frustum[index][3] <= 0.0)
          return false;
      }
      return true;
    }

    private void ExtractFrustum()
    {
      try
      {
        float[] data1 = new float[16];
        float[] data2 = new float[16];
        float[] numArray = new float[16];
        GL.GetFloat(GetPName.ProjectionMatrix, data1);
        GL.GetFloat(GetPName.Modelview0MatrixExt, data2);
        numArray[0] = (float) ((double) data2[0] * (double) data1[0] + (double) data2[1] * (double) data1[4] + (double) data2[2] * (double) data1[8] + (double) data2[3] * (double) data1[12]);
        numArray[1] = (float) ((double) data2[0] * (double) data1[1] + (double) data2[1] * (double) data1[5] + (double) data2[2] * (double) data1[9] + (double) data2[3] * (double) data1[13]);
        numArray[2] = (float) ((double) data2[0] * (double) data1[2] + (double) data2[1] * (double) data1[6] + (double) data2[2] * (double) data1[10] + (double) data2[3] * (double) data1[14]);
        numArray[3] = (float) ((double) data2[0] * (double) data1[3] + (double) data2[1] * (double) data1[7] + (double) data2[2] * (double) data1[11] + (double) data2[3] * (double) data1[15]);
        numArray[4] = (float) ((double) data2[4] * (double) data1[0] + (double) data2[5] * (double) data1[4] + (double) data2[6] * (double) data1[8] + (double) data2[7] * (double) data1[12]);
        numArray[5] = (float) ((double) data2[4] * (double) data1[1] + (double) data2[5] * (double) data1[5] + (double) data2[6] * (double) data1[9] + (double) data2[7] * (double) data1[13]);
        numArray[6] = (float) ((double) data2[4] * (double) data1[2] + (double) data2[5] * (double) data1[6] + (double) data2[6] * (double) data1[10] + (double) data2[7] * (double) data1[14]);
        numArray[7] = (float) ((double) data2[4] * (double) data1[3] + (double) data2[5] * (double) data1[7] + (double) data2[6] * (double) data1[11] + (double) data2[7] * (double) data1[15]);
        numArray[8] = (float) ((double) data2[8] * (double) data1[0] + (double) data2[9] * (double) data1[4] + (double) data2[10] * (double) data1[8] + (double) data2[11] * (double) data1[12]);
        numArray[9] = (float) ((double) data2[8] * (double) data1[1] + (double) data2[9] * (double) data1[5] + (double) data2[10] * (double) data1[9] + (double) data2[11] * (double) data1[13]);
        numArray[10] = (float) ((double) data2[8] * (double) data1[2] + (double) data2[9] * (double) data1[6] + (double) data2[10] * (double) data1[10] + (double) data2[11] * (double) data1[14]);
        numArray[11] = (float) ((double) data2[8] * (double) data1[3] + (double) data2[9] * (double) data1[7] + (double) data2[10] * (double) data1[11] + (double) data2[11] * (double) data1[15]);
        numArray[12] = (float) ((double) data2[12] * (double) data1[0] + (double) data2[13] * (double) data1[4] + (double) data2[14] * (double) data1[8] + (double) data2[15] * (double) data1[12]);
        numArray[13] = (float) ((double) data2[12] * (double) data1[1] + (double) data2[13] * (double) data1[5] + (double) data2[14] * (double) data1[9] + (double) data2[15] * (double) data1[13]);
        numArray[14] = (float) ((double) data2[12] * (double) data1[2] + (double) data2[13] * (double) data1[6] + (double) data2[14] * (double) data1[10] + (double) data2[15] * (double) data1[14]);
        numArray[15] = (float) ((double) data2[12] * (double) data1[3] + (double) data2[13] * (double) data1[7] + (double) data2[14] * (double) data1[11] + (double) data2[15] * (double) data1[15]);
        this.frustum[0][0] = numArray[3] - numArray[0];
        this.frustum[0][1] = numArray[7] - numArray[4];
        this.frustum[0][2] = numArray[11] - numArray[8];
        this.frustum[0][3] = numArray[15] - numArray[12];
        float num1 = (float) Math.Sqrt((double) this.frustum[0][0] * (double) this.frustum[0][0] + (double) this.frustum[0][1] * (double) this.frustum[0][1] + (double) this.frustum[0][2] * (double) this.frustum[0][2]);
        this.frustum[0][0] /= num1;
        this.frustum[0][1] /= num1;
        this.frustum[0][2] /= num1;
        this.frustum[0][3] /= num1;
        this.frustum[1][0] = numArray[3] + numArray[0];
        this.frustum[1][1] = numArray[7] + numArray[4];
        this.frustum[1][2] = numArray[11] + numArray[8];
        this.frustum[1][3] = numArray[15] + numArray[12];
        float num2 = (float) Math.Sqrt((double) this.frustum[1][0] * (double) this.frustum[1][0] + (double) this.frustum[1][1] * (double) this.frustum[1][1] + (double) this.frustum[1][2] * (double) this.frustum[1][2]);
        this.frustum[1][0] /= num2;
        this.frustum[1][1] /= num2;
        this.frustum[1][2] /= num2;
        this.frustum[1][3] /= num2;
        this.frustum[2][0] = numArray[3] + numArray[1];
        this.frustum[2][1] = numArray[7] + numArray[5];
        this.frustum[2][2] = numArray[11] + numArray[9];
        this.frustum[2][3] = numArray[15] + numArray[13];
        float num3 = (float) Math.Sqrt((double) this.frustum[2][0] * (double) this.frustum[2][0] + (double) this.frustum[2][1] * (double) this.frustum[2][1] + (double) this.frustum[2][2] * (double) this.frustum[2][2]);
        this.frustum[2][0] /= num3;
        this.frustum[2][1] /= num3;
        this.frustum[2][2] /= num3;
        this.frustum[2][3] /= num3;
        this.frustum[3][0] = numArray[3] - numArray[1];
        this.frustum[3][1] = numArray[7] - numArray[5];
        this.frustum[3][2] = numArray[11] - numArray[9];
        this.frustum[3][3] = numArray[15] - numArray[13];
        float num4 = (float) Math.Sqrt((double) this.frustum[3][0] * (double) this.frustum[3][0] + (double) this.frustum[3][1] * (double) this.frustum[3][1] + (double) this.frustum[3][2] * (double) this.frustum[3][2]);
        this.frustum[3][0] /= num4;
        this.frustum[3][1] /= num4;
        this.frustum[3][2] /= num4;
        this.frustum[3][3] /= num4;
        this.frustum[4][0] = numArray[3] - numArray[2];
        this.frustum[4][1] = numArray[7] - numArray[6];
        this.frustum[4][2] = numArray[11] - numArray[10];
        this.frustum[4][3] = numArray[15] - numArray[14];
        float num5 = (float) Math.Sqrt((double) this.frustum[4][0] * (double) this.frustum[4][0] + (double) this.frustum[4][1] * (double) this.frustum[4][1] + (double) this.frustum[4][2] * (double) this.frustum[4][2]);
        this.frustum[4][0] /= num5;
        this.frustum[4][1] /= num5;
        this.frustum[4][2] /= num5;
        this.frustum[4][3] /= num5;
        this.frustum[5][0] = numArray[3] + numArray[2];
        this.frustum[5][1] = numArray[7] + numArray[6];
        this.frustum[5][2] = numArray[11] + numArray[10];
        this.frustum[5][3] = numArray[15] + numArray[14];
        float num6 = (float) Math.Sqrt((double) this.frustum[5][0] * (double) this.frustum[5][0] + (double) this.frustum[5][1] * (double) this.frustum[5][1] + (double) this.frustum[5][2] * (double) this.frustum[5][2]);
        this.frustum[5][0] /= num6;
        this.frustum[5][1] /= num6;
        this.frustum[5][2] /= num6;
        this.frustum[5][3] /= num6;
      }
      catch (Exception ex)
      {
      }
    }

    private void DrawMusicZones(List<byte> F19250)
    {
    }

    public void RenderScene(string replacedModel, string replacedModelB)
    {
      this.levelDList = (uint) GL.GenLists(1);
      this.levelBDList = (uint) GL.GenLists(1);
      if (this.file != null)
      {
        GL.NewList(this.levelDList, ListMode.Compile);
        if (replacedModel == "")
        {
          if (this.file.modelAPointer != 0)
            this.file.bounds = Core.DrawModelFile(this.file.modelAPointer);
        }
        else
          this.file.bounds = Core.DrawModelFile(replacedModel);
        GL.Disable(EnableCap.Texture2D);
        GL.EndList();
        GL.NewList(this.levelBDList, ListMode.Compile);
        if (replacedModel == "")
        {
          if (this.file.modelBPointer != 0)
            this.file.boundsAlphaModel = Core.DrawModelFile(this.file.modelBPointer);
        }
        else if (replacedModelB != "")
          this.file.boundsAlphaModel = Core.DrawModelFile(replacedModelB);
        GL.Disable(EnableCap.Texture2D);
        GL.EndList();
        this.movementDL = (uint) GL.GenLists(1);
        GL.NewList(this.movementDL, ListMode.Compile);
        GL.Disable(EnableCap.Texture2D);
        GL.EndList();
      }
      this.DrawLevelBoundary();
    }

    public void RenderPaths()
    {
      try
      {
        for (int index = 0; index < this.pathsDList.Count; ++index)
          GL.DeleteLists(this.pathsDList[index], 1);
      }
      catch
      {
      }
      this.pathsDList.Clear();
      for (int index = 0; index < this.paths.Count; ++index)
      {
        uint list = (uint) GL.GenLists(1);
        GL.NewList(list, ListMode.Compile);
        this.DrawPath(this.paths[index], false);
        GL.EndList();
        this.pathsDList.Add(list);
      }
    }

    public void redrawSelectedPath()
    {
      GL.DeleteLists(this.pathsDList[this.selectedPath], 1);
      GL.NewList((uint) GL.GenLists(1), ListMode.Compile);
      this.DrawPath(this.paths[this.selectedPath], false);
      GL.EndList();
    }

    private void DrawPath(BKPath p, bool s)
    {
      if (p.pathObject != -1)
      {
        GL.PushMatrix();
        this.SetupObjectMatrix(this.objects[p.pathObject]);
        int num = (int) Core.DrawObject(this.objects[p.pathObject], createDL: false);
        GL.PopMatrix();
        GL.Disable(EnableCap.Texture2D);
      }
      for (int index1 = 0; index1 < p.nodes.Count; ++index1)
      {
        p.nodes[index1].bb = new BoundingBox();
        p.nodes[index1].bb.smallX = -25;
        p.nodes[index1].bb.smallY = -25;
        p.nodes[index1].bb.smallZ = -25;
        p.nodes[index1].bb.largeX = 25;
        p.nodes[index1].bb.largeY = 25;
        p.nodes[index1].bb.largeZ = 25;
        if (p.nodes[index1].type == ObjectType.Path)
        {
          GL.Color3(1f, 1f, 1f);
          GL.PushMatrix();
          this.SetupObjectMatrix(p.nodes[index1]);
          Core.DrawPri(p.nodes[index1].radius, p.nodes[index1].colour);
          GL.PopMatrix();
        }
        GL.PushMatrix();
        if (s)
          GL.Color3(0.3f, 0.3f, 1f);
        else
          GL.Color3(1, 1, 1);
        GL.Begin(BeginMode.Lines);
        for (int index2 = 0; index2 < p.nodes.Count; ++index2)
        {
          if ((int) p.nodes[index1].nodeOutUID == (int) p.nodes[index2].uid && p.nodes[index2].type != ObjectType.SPath)
          {
            GL.Color3(0.95f, 0.51f, 0.14f);
            GL.Vertex3(p.nodes[index2].locX, p.nodes[index2].locY, p.nodes[index2].locZ);
            GL.Vertex3(p.nodes[index1].locX, p.nodes[index1].locY, p.nodes[index1].locZ);
          }
          if (p.pathObject != -1 && (int) this.objects[p.pathObject].nodeOutUID == (int) p.nodes[index2].uid)
          {
            GL.Color3(1f, 1f, 1f);
            if (p.nodes[index2].type == ObjectType.SPath)
              GL.Color3(1, 0, 0);
            GL.Vertex3(p.nodes[index2].locX, p.nodes[index2].locY, p.nodes[index2].locZ);
            GL.Vertex3(this.objects[p.pathObject].locX, this.objects[p.pathObject].locY, this.objects[p.pathObject].locZ);
          }
        }
        GL.End();
        GL.PopMatrix();
      }
    }

    public void renderPathPicking()
    {
      try
      {
        GL.DeleteLists(this.pickPathDList, 1);
      }
      catch
      {
      }
      this.pickPathDList = (uint) GL.GenLists(1);
      GL.NewList(this.pickPathDList, ListMode.Compile);
      GL.Disable(EnableCap.CullFace);
      try
      {
        for (int index1 = 0; index1 < this.paths.Count; ++index1)
        {
          GL.PushMatrix();
          if (this.paths[index1].pathObject != -1)
          {
            GL.PushMatrix();
            ObjectData objectData = this.objects[this.paths[index1].pathObject];
            this.SetupObjectMatrix(objectData);
            int num = (int) Core.DrawObject(objectData, true, false, (float) this.paths[index1].m_colorID[0] / (float) byte.MaxValue, (float) this.paths[index1].m_colorID[1] / (float) byte.MaxValue, (float) this.paths[index1].m_colorID[2] / (float) byte.MaxValue);
            GL.PopMatrix();
          }
          for (int index2 = 0; index2 < this.paths[index1].nodes.Count; ++index2)
          {
            if (this.paths[index1].nodes[index2].type == ObjectType.Path)
            {
              GL.Color3((float) this.paths[index1].m_colorID[0] / (float) byte.MaxValue, (float) this.paths[index1].m_colorID[1] / (float) byte.MaxValue, (float) this.paths[index1].m_colorID[2] / (float) byte.MaxValue);
              GL.PushMatrix();
              this.SetupObjectMatrix(this.paths[index1].nodes[index2]);
              Core.DrawPriPick();
              GL.PopMatrix();
            }
          }
          GL.PopMatrix();
        }
      }
      catch (Exception ex)
      {
        World.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      GL.EndList();
      try
      {
        GL.DeleteLists(this.pickPathNodeDList, 1);
      }
      catch
      {
      }
      this.pickPathNodeDList = (uint) GL.GenLists(1);
      GL.NewList(this.pickPathNodeDList, ListMode.Compile);
      GL.Disable(EnableCap.CullFace);
      for (int index3 = 0; index3 < this.paths.Count; ++index3)
      {
        for (int index4 = 0; index4 < this.paths[index3].nodes.Count; ++index4)
        {
          if (this.paths[index3].nodes[index4].type == ObjectType.Path)
          {
            GL.Color3((float) this.paths[index3].nodes[index4].m_colorID[0] / (float) byte.MaxValue, (float) this.paths[index3].nodes[index4].m_colorID[1] / (float) byte.MaxValue, (float) this.paths[index3].nodes[index4].m_colorID[2] / (float) byte.MaxValue);
            GL.PushMatrix();
            this.SetupObjectMatrix(this.paths[index3].nodes[index4]);
            Core.DrawPriPick();
            GL.PopMatrix();
          }
        }
      }
      GL.EndList();
      GL.Flush();
    }

    public void renderSetup()
    {
      this.RenderObjects();
      this.RenderStructs();
      this.RenderCameras();
    }

    public void RenderObjects()
    {
      try
      {
        for (int index = 0; index < this.objectsDList.Count; ++index)
          GL.DeleteLists(this.objectsDList[index], 1);
        for (int index = 0; index < this.objectsPickingDList.Count; ++index)
          GL.DeleteLists(this.objectsPickingDList[index], 1);
      }
      catch
      {
      }
      this.objectsDList.Clear();
      this.objectsPickingDList.Clear();
      foreach (ObjectData objectData in this.objects)
      {
        this.objectsDList.Add(Core.DrawObject(objectData));
        this.objectsPickingDList.Add(Core.DrawObject(objectData, true));
      }
    }

    public void RenderStructs()
    {
      try
      {
        for (int index = 0; index < this.structsDList.Count; ++index)
          GL.DeleteLists(this.structsDList[index], 1);
        for (int index = 0; index < this.structsPickingDList.Count; ++index)
          GL.DeleteLists(this.structsPickingDList[index], 1);
      }
      catch
      {
      }
      this.structsDList.Clear();
      this.structsPickingDList.Clear();
      foreach (ObjectData objectData in this.structs)
      {
        this.structsDList.Add(Core.DrawObject(objectData));
        this.structsPickingDList.Add(Core.DrawObject(objectData, true));
      }
    }

    private void RenderCameras()
    {
      try
      {
        for (int index = 0; index < this.cameraDList.Count; ++index)
          GL.DeleteLists(this.cameraDList[index], 1);
      }
      catch
      {
      }
      this.cameraDList.Clear();
      for (int index = 0; index < this.cameras.Count; ++index)
      {
        CameraObject camera = this.cameras[index];
        uint list = (uint) GL.GenLists(1);
        GL.NewList(list, ListMode.Compile);
        Core.DrawCamera(camera);
        GL.EndList();
        GL.Flush();
        this.cameraDList.Add(list);
      }
    }

    public void RenderCameraPicking()
    {
      try
      {
        for (int index = 0; index < this.cameraPickDList.Count; ++index)
          GL.DeleteLists(this.cameraPickDList[index], 1);
      }
      catch
      {
      }
      this.cameraPickDList.Clear();
      for (int index = 0; index < this.cameras.Count; ++index)
        this.cameraPickDList.Add(Core.DrawCameraPicking(this.cameras[index]));
    }

    public void renderPicking()
    {
      this.renderPathPicking();
      this.RenderCameraPicking();
    }

    public int pickPath(int x, int y, bool multiselect, GLCamera cam)
    {
      Core.ClearScreenAndLoadIdentity();
      GL.PushMatrix();
      GL.LoadMatrix(cam.GetWorldToViewMatrix());
      this.ResetPick();
      this.renderPathPicking();
      byte[] numArray = Core.BackBufferSelect(x, y, this.pickPathDList);
      int num = -1;
      for (int index = 0; index < this.paths.Count; ++index)
      {
        if ((int) this.paths[index].m_colorID[0] == (int) numArray[0] && (int) this.paths[index].m_colorID[1] == (int) numArray[1] && (int) this.paths[index].m_colorID[2] == (int) numArray[2])
          num = index;
      }
      this.selectedPath = num;
      this.selectedNodes.Clear();
      this.selectedSNodes.Clear();
      GL.PopMatrix();
      this.pickPathNode(x, y, multiselect, cam);
      return num;
    }

    public void pickPathNode(int x, int y, bool multiselect, GLCamera cam)
    {
      Core.ClearScreenAndLoadIdentity();
      GL.PushMatrix();
      GL.LoadMatrix(cam.GetWorldToViewMatrix());
      if (this.selectedPath != -1)
      {
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        GL.Disable(EnableCap.Texture2D);
        GL.Disable(EnableCap.Fog);
        GL.Disable(EnableCap.Lighting);
        GL.ShadeModel(ShadingModel.Flat);
        GL.Disable(EnableCap.CullFace);
        try
        {
          foreach (ObjectData node in this.paths[this.selectedPath].nodes)
          {
            if (node.type == ObjectType.Path)
            {
              GL.Color3((float) node.m_colorID[0] / (float) byte.MaxValue, (float) node.m_colorID[1] / (float) byte.MaxValue, (float) node.m_colorID[2] / (float) byte.MaxValue);
              GL.PushMatrix();
              GL.Translate((float) node.locX, (float) node.locY, (float) node.locZ);
              Core.DrawPriPick();
              GL.PopMatrix();
            }
          }
        }
        catch (Exception ex)
        {
        }
        byte[] pixels = new byte[3];
        int[] data = new int[4];
        GL.GetInteger(GetPName.Viewport, data);
        GL.ReadPixels<byte>(x, data[3] - y, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixels);
        if (!multiselect)
          this.selectedNodes.Clear();
        this.selectedSNodes.Clear();
        for (int index = 0; index < this.paths[this.selectedPath].nodes.Count; ++index)
        {
          if ((int) this.paths[this.selectedPath].nodes[index].m_colorID[0] == (int) pixels[0] && (int) this.paths[this.selectedPath].nodes[index].m_colorID[1] == (int) pixels[1] && (int) this.paths[this.selectedPath].nodes[index].m_colorID[2] == (int) pixels[2] && this.paths[this.selectedPath].nodes[index].type != ObjectType.SPath)
            this.selectedNodes.Add(index);
        }
      }
      GL.PopMatrix();
    }

    public bool linkNode(int x, int y, GLCamera cam)
    {
      Core.ClearScreenAndLoadIdentity();
      GL.PushMatrix();
      GL.LoadMatrix(cam.GetWorldToViewMatrix());
      bool flag1 = false;
      if (this.selectedPath != -1)
      {
        int index1 = -1;
        bool flag2 = false;
        if (this.selectedNodes.Count == 1)
          index1 = this.selectedNodes[0];
        else if (this.selectedSNodes.Count == 1)
        {
          index1 = this.selectedSNodes[0];
          flag2 = true;
        }
        if (index1 != -1)
        {
          this.pickPathNode(x, y, false, cam);
          int index2 = -1;
          if (this.selectedNodes.Count > 0)
          {
            if (this.selectedNodes.Count<int>() == 1)
              index2 = this.selectedNodes[0];
            if (this.selectedSNodes.Count<int>() == 1)
              index2 = this.selectedSNodes[0];
            if (this.paths[this.selectedPath].nodes[index1].nodeOutUID != (short) 0)
            {
              foreach (ObjectData node in this.paths[this.selectedPath].nodes)
              {
                if ((int) this.paths[this.selectedPath].nodes[index1].nodeOutUID == (int) node.uid)
                  node.node_in = false;
              }
            }
            this.paths[this.selectedPath].nodes[index1].nodeOutUID = this.paths[this.selectedPath].nodes[index2].uid;
            this.paths[this.selectedPath].nodes[index1].obj18 = (byte) ((int) this.paths[this.selectedPath].nodes[index1].nodeOutUID / 16 & (int) byte.MaxValue);
            this.paths[this.selectedPath].nodes[index1].node_out = true;
            this.paths[this.selectedPath].nodes[index2].node_in = true;
            flag1 = true;
            this.selectedNodes.Clear();
            this.selectedSNodes.Clear();
            if (flag2)
              this.selectedSNodes.Add(index1);
            else
              this.selectedNodes.Add(index1);
          }
          this.selectedNodes.Clear();
          this.selectedSNodes.Clear();
          if (flag2)
            this.selectedSNodes.Add(index1);
          else
            this.selectedNodes.Add(index1);
        }
      }
      GL.PopMatrix();
      return flag1;
    }

    public void pickObjectForPath(int x, int y, GLCamera cam)
    {
      try
      {
        Core.ClearScreenAndLoadIdentity();
        GL.PushMatrix();
        GL.LoadMatrix(cam.GetWorldToViewMatrix());
        if (this.selectedPath != -1 && this.objects.Count > 0 && this.paths[this.selectedPath].nodes.Count > 0)
        {
          int index1 = -1;
          for (int index2 = 0; index2 < this.objects.Count; ++index2)
            this.DrawObjectDisplayList(this.objects[index2], this.objectsPickingDList[index2], cam);
          byte[] numArray = Core.BackBufferSelect(x, y);
          for (int index3 = 0; index3 < this.objects.Count; ++index3)
          {
            if (this.objects[index3].name != "" && this.objects[index3].type == ObjectType.Normal && (int) this.objects[index3].m_colorID[0] == (int) numArray[0] && (int) this.objects[index3].m_colorID[1] == (int) numArray[1] && (int) this.objects[index3].m_colorID[2] == (int) numArray[2])
            {
              index1 = index3;
              break;
            }
          }
          if (index1 == -1)
            return;
          if (this.paths[this.selectedPath].pathObject != -1)
          {
            this.objects[index1].obj16 = this.objects[this.paths[this.selectedPath].pathObject].obj16;
            this.objects[index1].obj17 = this.objects[this.paths[this.selectedPath].pathObject].obj17;
            this.objects[index1].uid = this.objects[this.paths[this.selectedPath].pathObject].uid;
            this.objects[this.paths[this.selectedPath].pathObject].obj16 = (byte) 0;
            this.objects[this.paths[this.selectedPath].pathObject].obj17 = (byte) 0;
            this.objects[this.paths[this.selectedPath].pathObject].obj18 = (byte) 0;
            this.objects[this.paths[this.selectedPath].pathObject].nodeOutUID = (short) 0;
            this.objects[this.paths[this.selectedPath].pathObject].uid = (short) 0;
          }
          if (!this.paths[this.selectedPath].IsPathLooped())
          {
            int firstNode = this.paths[this.selectedPath].GetFirstNode();
            if (firstNode != -1)
              this.LinkObjectToNode(this.objects[index1], firstNode);
          }
          else if (this.selectedNodes.Count > 0)
            this.LinkObjectToNode(this.objects[index1], this.selectedNodes[0]);
          else
            this.LinkObjectToNode(this.objects[index1], this.selectedSNodes[0]);
          this.paths[this.selectedPath].pathObject = index1;
          this.redrawSelectedPath();
          this.renderPicking();
        }
        this.pmode = PathMode.None;
        GL.PopMatrix();
        this.pushSetupStack("PathMode: Assigned Object");
      }
      catch (Exception ex)
      {
        World.logger.Log(NLog.LogLevel.Error, "Failed to pick object for path");
        World.logger.Log(NLog.LogLevel.Debug, ex.Message.ToString());
      }
    }

    private void LinkObjectToNode(ObjectData o, int node)
    {
      o.nodeOutUID = this.paths[this.selectedPath].nodes[node].uid;
      o.obj18 = (byte) ((int) o.nodeOutUID / 16 & (int) byte.MaxValue);
      this.paths[this.selectedPath].nodes[node].node_in = true;
    }

    public void pickColorOBJ(int x_, int y, bool multiselect, GLCamera cam)
    {
      if (!multiselect)
        this.ResetPick();
      for (int index = 0; index < this.objects.Count; ++index)
        this.DrawObjectDisplayList(this.objects[index], this.objectsPickingDList[index], cam);
      for (int index = 0; index < this.structs.Count; ++index)
        this.DrawObjectDisplayList(this.structs[index], this.structsPickingDList[index], cam);
      this.selectBackBufferObject(Core.BackBufferSelect(x_, y));
    }

    public void pickRectSelect(Point p1, Point p2, Mode mode, GLCamera cam)
    {
      try
      {
        if (mode == Mode.Path && this.selectedPath == -1)
          return;
        BKPath bkPath = new BKPath();
        if (mode == Mode.Path && this.selectedPath != -1)
          bkPath = this.paths[this.selectedPath];
        GL.Disable(EnableCap.Texture2D);
        GL.Disable(EnableCap.Fog);
        GL.Disable(EnableCap.Lighting);
        GL.ShadeModel(ShadingModel.Flat);
        if (mode == Mode.Object)
        {
          for (int index = 0; index < this.objects.Count; ++index)
            this.DrawObjectDisplayList(this.objects[index], this.objectsPickingDList[index], cam);
          for (int index = 0; index < this.structs.Count; ++index)
            this.DrawObjectDisplayList(this.structs[index], this.structsPickingDList[index], cam);
        }
        if (mode == Mode.Path)
        {
          foreach (ObjectData node in bkPath.nodes)
          {
            GL.Color3((float) node.m_colorID[0] / (float) byte.MaxValue, (float) node.m_colorID[1] / (float) byte.MaxValue, (float) node.m_colorID[2] / (float) byte.MaxValue);
            GL.PushMatrix();
            GL.Translate((float) node.locX, (float) node.locY, (float) node.locZ);
            Core.DrawPriPick();
            GL.PopMatrix();
          }
        }
        byte[] numArray = Core.pickRectSelect(p1, p2);
        List<int> intList = new List<int>();
        int num1 = (p2.X - p1.X + 1) * (p2.Y - p1.Y + 1) * 3 + 3;
        for (int index = 0; index < num1; index += 3)
        {
          int num2 = (int) numArray[index] + ((int) numArray[index + 1] << 8) + ((int) numArray[index + 2] << 16);
          if (num2 > 0 && !intList.Contains(num2))
          {
            if (mode == Mode.Object)
              this.selectBackBufferObject(new byte[3]
              {
                numArray[index],
                numArray[index + 1],
                numArray[index + 2]
              });
            if (mode == Mode.Path)
              this.selectBackBufferNode(new byte[3]
              {
                numArray[index],
                numArray[index + 1],
                numArray[index + 2]
              });
            intList.Add(num2);
          }
        }
      }
      catch (Exception ex)
      {
        World.logger.Log(NLog.LogLevel.Debug, ex.Message);
        World.logger.Log(NLog.LogLevel.Error, "Couldn't Select a path node");
      }
    }

    public void selectBackBufferObject(byte[] pixel)
    {
      int index1 = -1;
      int num = -1;
      for (int index2 = 0; index2 < this.objects.Count; ++index2)
      {
        ObjectData objectData = this.objects[index2];
        if ((int) objectData.m_colorID[0] == (int) pixel[0] && (int) objectData.m_colorID[1] == (int) pixel[1] && (int) objectData.m_colorID[2] == (int) pixel[2])
        {
          index1 = index2;
          num = 0;
          this.SetInitialObjectValues(this.objects[index1]);
        }
      }
      if (index1 == -1)
      {
        for (int index3 = 0; index3 < this.structs.Count; ++index3)
        {
          ObjectData objectData = this.structs[index3];
          if ((int) objectData.m_colorID[0] == (int) pixel[0] && (int) objectData.m_colorID[1] == (int) pixel[1] && (int) objectData.m_colorID[2] == (int) pixel[2])
          {
            index1 = index3;
            break;
          }
        }
        if (index1 != -1)
        {
          num = 1;
          this.SetInitialObjectValues(this.structs[index1]);
        }
      }
      if (index1 == -1)
        return;
      if (num == 0)
      {
        if (this.selectedObjects.Contains(index1))
          return;
        this.selectedObjects.Add(index1);
      }
      else
      {
        if (this.selectedStructs.Contains(index1))
          return;
        this.selectedStructs.Add(index1);
      }
    }

    public void selectBackBufferNode(byte[] pixel)
    {
      if (this.selectedPath == -1)
        return;
      for (int index = 0; index < this.paths[this.selectedPath].nodes.Count; ++index)
      {
        if (!this.selectedNodes.Contains(index) && !this.selectedSNodes.Contains(index) && (int) this.paths[this.selectedPath].nodes[index].m_colorID[0] == (int) pixel[0] && (int) this.paths[this.selectedPath].nodes[index].m_colorID[1] == (int) pixel[1] && (int) this.paths[this.selectedPath].nodes[index].m_colorID[2] == (int) pixel[2] && this.paths[this.selectedPath].nodes[index].type != ObjectType.SPath)
          this.selectedNodes.Add(index);
      }
    }

    public void pickCamera(int x, int y, GLCamera BBCamera)
    {
      try
      {
        this.ResetPick();
        Core.ClearScreenAndLoadIdentity();
        GL.PushMatrix();
        GL.LoadMatrix(BBCamera.GetWorldToViewMatrix());
        for (int index = 0; index < this.cameraPickDList.Count<uint>(); ++index)
        {
          CameraObject camera = this.cameras[index];
          GL.PushMatrix();
          if (camera.type == 2)
          {
            GL.Translate(camera.x, camera.y, camera.z);
            GL.Rotate(camera.roll, 0.0f, 0.0f, 1f);
            GL.Rotate(camera.yaw, 0.0f, 1f, 0.0f);
            GL.Rotate(camera.pitch, 1f, 0.0f, 0.0f);
          }
          if (camera.type == 3 || camera.type == 1)
            GL.Translate(camera.x, camera.y, camera.z);
          GL.CallList(this.cameraPickDList[index]);
          GL.PopMatrix();
        }
        byte[] numArray = Core.BackBufferSelect(x, y);
        for (int index = 0; index < this.cameras.Count; ++index)
        {
          CameraObject camera = this.cameras[index];
          if ((int) camera.m_colorID[0] == (int) numArray[0] && (int) camera.m_colorID[1] == (int) numArray[1] && (int) camera.m_colorID[2] == (int) numArray[2])
            this.selectedCam = index;
        }
        GL.PopMatrix();
        Core.InitGl();
      }
      catch (Exception ex)
      {
        World.logger.Log(NLog.LogLevel.Error, "Failed to pick camera");
        World.logger.Log(NLog.LogLevel.Debug, ex.Message.ToString());
      }
    }

    public void SetInitialObjectValues(ObjectData obj)
    {
      this.oldR = obj.rot;
      this.oldS = obj.size;
      this.oldX = obj.locX;
      this.oldY = obj.locY;
      this.oldZ = obj.locZ;
      this.oldRad = obj.radius;
    }

    public double[] ScreenToWorld(int x, int y, GLCamera cam)
    {
      Core.ClearScreenAndLoadIdentity();
      GL.PushMatrix();
      GL.LoadMatrix(cam.GetWorldToViewMatrix());
      GL.CallList(this.levelDList);
      GL.CallList(this.levelBDList);
      int[] numArray1 = new int[4];
      double[] numArray2 = new double[16];
      double[] numArray3 = new double[16];
      GL.GetInteger(GetPName.Viewport, numArray1);
      GL.GetDouble(GetPName.Modelview0MatrixExt, numArray2);
      GL.GetDouble(GetPName.ProjectionMatrix, numArray3);
      int num1 = numArray1[3];
      int x1 = x;
      int num2 = y;
      int y1 = num1 - num2;
      float[] pixels = new float[1];
      GL.ReadPixels<float>(x1, y1, 1, 1, PixelFormat.DepthComponent, PixelType.Float, pixels);
      double objX = 0.0;
      double objY = 0.0;
      double objZ = 0.0;
      Glu.gluUnProject((double) x1, (double) y1, (double) pixels[0], numArray2, numArray3, numArray1, out objX, out objY, out objZ);
      GL.PopMatrix();
      if ((double) pixels[0] == 1.0)
      {
        GL.PushMatrix();
        GL.Translate(0.0f, 0.0f, -2000f);
        Core.DrawInvisibleWall();
        GL.PopMatrix();
        GL.ReadPixels<float>(x1, y1, 1, 1, PixelFormat.DepthComponent, PixelType.Float, pixels);
        objX = 0.0;
        objY = 0.0;
        objZ = 0.0;
        Glu.gluUnProject((double) x1, (double) y1, (double) pixels[0], numArray2, numArray3, numArray1, out objX, out objY, out objZ);
      }
      return new double[3]{ objX, objY, objZ };
    }

    public void ReadSetupFile(string file_)
    {
      this.setupFileReader.init(file_);
      this.objects = this.setupFileReader.ReadSetupFile();
      for (int index1 = 0; index1 < this.objects.Count; ++index1)
      {
        if (this.objects[index1].type == ObjectType.SPath)
        {
          for (int index2 = 0; index2 < this.objects.Count; ++index2)
          {
            if (this.objects[index2].type == ObjectType.Path && (((int) this.objects[index2].obj16 * 256 + (int) this.objects[index2].obj17) / 16 & (int) byte.MaxValue) == (int) this.objects[index1].obj18)
            {
              this.objects[index1].locX = this.objects[index2].locX;
              this.objects[index1].locY = this.objects[index2].locY;
              this.objects[index1].locZ = this.objects[index2].locZ;
            }
          }
        }
      }
    }

    private void SetSPathLocations()
    {
      for (int index1 = 0; index1 < this.paths.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.paths[index1].nodes.Count; ++index2)
        {
          if (this.paths[index1].nodes[index2].type == ObjectType.SPath)
          {
            if (index2 == 0)
            {
              for (int index3 = 0; index3 < this.paths[index1].nodes.Count; ++index3)
              {
                if (this.paths[index1].nodes[index3].type == ObjectType.Path)
                {
                  this.paths[index1].nodes[index2].locX = this.paths[index1].nodes[index3].locX;
                  this.paths[index1].nodes[index2].locY = this.paths[index1].nodes[index3].locY;
                  this.paths[index1].nodes[index2].locZ = this.paths[index1].nodes[index3].locZ;
                  break;
                }
              }
            }
            else
            {
              this.paths[index1].nodes[index2].locX = this.paths[index1].nodes[index2 - 1].locX;
              this.paths[index1].nodes[index2].locY = this.paths[index1].nodes[index2 - 1].locY;
              this.paths[index1].nodes[index2].locZ = this.paths[index1].nodes[index2 - 1].locZ;
            }
          }
        }
      }
    }

    private void DrawSelectedPathController()
    {
      try
      {
        List<int> intList1 = new List<int>();
        List<int> intList2 = new List<int>();
        short nodeOutUid = this.objects[this.paths[this.selectedPath].pathObject].nodeOutUID;
        for (int index = 0; index < this.paths[this.selectedPath].nodes.Count; ++index)
        {
          ObjectData node = this.paths[this.selectedPath].nodes[index];
          if ((int) node.uid == (int) nodeOutUid)
          {
            if (node.type == ObjectType.Path)
              intList1.Add(index);
            intList2.Add((int) node.uid);
            nodeOutUid = node.nodeOutUID;
            if (!intList2.Contains((int) nodeOutUid))
              index = -1;
            else
              break;
          }
        }
        double num1;
        int index1 = (int) Math.Floor(num1 = (double) this.paths[this.selectedPath].nodes[this.selectedSNodes[0]].activationPercent * (double) (intList1.Count - 1));
        float num2 = (float) (num1 % 1.0);
        ObjectData node1 = this.paths[this.selectedPath].nodes[intList1[index1]];
        ObjectData node2 = this.paths[this.selectedPath].nodes[intList1[index1]];
        ObjectData objectData = index1 != intList1.Count - 1 ? this.paths[this.selectedPath].nodes[intList1[index1 + 1]] : this.paths[this.selectedPath].nodes[intList1[0]];
        float num3 = (float) ((int) objectData.locX - (int) node1.locX);
        float num4 = (float) ((int) objectData.locY - (int) node1.locY);
        float num5 = (float) ((int) objectData.locZ - (int) node1.locZ);
        float num6 = num3 * num2;
        float num7 = num4 * num2;
        float num8 = num5 * num2;
        Core.DrawPathController((float) node1.locX + num6, (float) node1.locY + num7, (float) node1.locZ + num8);
      }
      catch
      {
      }
    }

    private void ExtractPaths()
    {
      try
      {
        List<ObjectData> nodes = new List<ObjectData>();
        for (int index = 0; index < this.objects.Count; ++index)
        {
          this.UIDs.Add(this.objects[index].uid);
          if (this.objects[index].type == ObjectType.Path || this.objects[index].type == ObjectType.SPath)
          {
            nodes.Add(this.objects[index]);
            this.objects.RemoveAt(index);
            --index;
          }
        }
        for (int index = 0; index < this.objects.Count; ++index)
        {
          byte id = byte.MaxValue;
          if (this.objects[index].obj18 > (byte) 0)
          {
            BKPath bkPath = new BKPath();
            bkPath.pathObject = index;
            ObjectData nodesPartner1 = this.FindNodesPartner(nodes, this.objects[index], this.objects[index].obj18);
            if (nodesPartner1 != null)
            {
              if (nodesPartner1.type == ObjectType.SPath)
                id = nodesPartner1.pathID;
              nodesPartner1.node_in = true;
              bkPath.nodes.Add(nodesPartner1);
              this.objects[index].nodeOutUID = nodesPartner1.uid;
              bool flag = false;
              while (!flag)
              {
                ObjectData nodesPartner2 = this.FindNodesPartner(nodes, bkPath.nodes[bkPath.nodes.Count - 1], bkPath.nodes[bkPath.nodes.Count - 1].obj18);
                if (nodesPartner2 != null)
                {
                  bkPath.nodes[bkPath.nodes.Count - 1].nodeOutUID = nodesPartner2.uid;
                  bkPath.nodes.Add(nodesPartner2);
                  if (nodesPartner2.type == ObjectType.SPath)
                    id = nodesPartner2.pathID;
                }
                else
                  flag = true;
              }
              if (id != byte.MaxValue && id != (byte) 0)
                bkPath.nodes.AddRange((IEnumerable<ObjectData>) this.FindSNodes(nodes, id));
              if (bkPath.nodes.Count > 0)
                this.paths.Add(bkPath);
            }
          }
        }
        for (int index1 = 0; index1 < nodes.Count; ++index1)
        {
          List<int> matches = new List<int>();
          for (int index2 = 0; index2 < nodes.Count; ++index2)
          {
            if ((int) nodes[index1].obj18 == (((int) nodes[index2].obj16 * 256 + (int) nodes[index2].obj17) / 16 & (int) byte.MaxValue) && !nodes[index2].node_in)
              matches.Add(index2);
          }
          if (matches.Count > 0)
          {
            int bestNodeMatch = this.FindBestNodeMatch(nodes, nodes[index1], matches);
            nodes[matches[bestNodeMatch]].node_in = true;
            nodes[index1].nodeOutUID = nodes[matches[bestNodeMatch]].uid;
          }
          matches.Clear();
        }
        while (nodes.Count > 0)
        {
          BKPath bkPath = new BKPath();
          bkPath.nodes.Add(nodes[0]);
          nodes.RemoveAt(0);
          for (int index3 = 0; index3 < nodes.Count; ++index3)
          {
            bool flag = false;
            for (int index4 = 0; index4 < bkPath.nodes.Count && !flag; ++index4)
            {
              if ((int) bkPath.nodes[index4].nodeOutUID == (int) nodes[index3].uid)
              {
                bkPath.nodes.Add(nodes[index3]);
                nodes.RemoveAt(index3);
                index3 = -1;
                flag = true;
              }
              if (!flag && (int) nodes[index3].nodeOutUID == (int) bkPath.nodes[index4].uid)
              {
                bkPath.nodes.Add(nodes[index3]);
                nodes.RemoveAt(index3);
                index3 = -1;
                flag = true;
              }
            }
          }
          this.paths.Add(bkPath);
        }
        for (int index = 0; index < this.paths.Count; ++index)
        {
          if (this.paths[index].nodes[this.paths[index].nodes.Count - 1].obj18 != (byte) 0)
          {
            ObjectData node = this.paths[index].nodes[this.paths[index].nodes.Count - 1];
            node.node_out = true;
            node.nodeOutUID = (short) ((int) node.obj18 << 4);
          }
        }
      }
      catch (Exception ex)
      {
        World.logger.Log(NLog.LogLevel.Error, "Extracting Paths from setup failed");
        World.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
    }

    private List<ObjectData> FindSNodes(List<ObjectData> nodes, byte id)
    {
      List<ObjectData> objectDataList = new List<ObjectData>();
      for (int index = 0; index < nodes.Count; ++index)
      {
        if (nodes[index].type == ObjectType.SPath && (int) id == (int) nodes[index].pathID)
        {
          objectDataList.Add(nodes[index]);
          nodes.RemoveAt(index);
          --index;
        }
      }
      return objectDataList;
    }

    private ObjectData FindNodesPartner(List<ObjectData> nodes, ObjectData node, byte to)
    {
      List<int> matches = new List<int>();
      for (int index = 0; index < nodes.Count; ++index)
      {
        if ((int) to == ((int) nodes[index].uid / 16 & (int) byte.MaxValue))
          matches.Add(index);
      }
      if (matches.Count <= 0)
        return (ObjectData) null;
      int bestNodeMatch = this.FindBestNodeMatch(nodes, node, matches);
      ObjectData node1 = nodes[matches[bestNodeMatch]];
      nodes.RemoveAt(matches[bestNodeMatch]);
      return node1;
    }

    private int FindBestNodeMatch(List<ObjectData> nodes, ObjectData node, List<int> matches)
    {
      int num1 = 0;
      if (matches.Count > 0)
      {
        ObjectData node1 = nodes[matches[0]];
        float num2 = float.MaxValue;
        for (int index = 0; index < matches.Count; ++index)
        {
          float num3 = (float) Math.Sqrt(Math.Pow((double) ((int) nodes[matches[index]].locX - (int) node.locX), 2.0) + Math.Pow((double) ((int) nodes[matches[index]].locY - (int) node.locY), 2.0) + Math.Pow((double) ((int) nodes[matches[index]].locZ - (int) node.locZ), 2.0));
          if ((double) num2 > (double) num3)
          {
            num2 = num3;
            ObjectData node2 = nodes[matches[index]];
            num1 = index;
          }
        }
      }
      return num1;
    }

    public int GetListDec(string file_) => this.setupFileReader.GetListDec(file_, 0);

    public void AddObj(ObjectData data_)
    {
      this.objectsDList.Add(Core.DrawObject(data_));
      this.objectsPickingDList.Add(Core.DrawObject(data_, true));
      this.objects.Add(data_);
    }

    public void AddStruct(ObjectData data_)
    {
      this.structsDList.Add(Core.DrawObject(data_));
      this.structsPickingDList.Add(Core.DrawObject(data_, true));
      this.structs.Add(data_);
    }

    public void DeleteAllStructs() => this.structs.Clear();

    public void DeleteAllCameras() => this.cameras.Clear();

    public void DeleteAllObjects()
    {
      this.usedJiggyFlags.Clear();
      this.usedMTFlags.Clear();
      this.usedHCFlags.Clear();
      this.objects.Clear();
      this.selectedCam = -1;
      this.selectedObjects.Clear();
      this.selectedStructs.Clear();
      this.paths.Clear();
      this.UIDs.Clear();
      this.PathIDs.Clear();
      this.selectedPath = -1;
    }

    public void DeleteSelectedObjects()
    {
      this.selectedObjects.Sort();
      this.selectedObjects.Reverse();
      this.selectedStructs.Sort();
      this.selectedStructs.Reverse();
      foreach (int selectedObject in this.selectedObjects)
      {
        if (((IEnumerable<int>) new int[3]
        {
          70,
          45,
          71
        }).Contains<int>((int) this.objects[selectedObject].objectID) && this.objects[selectedObject].flag != -1)
        {
          for (int index = 0; index < this.objects.Count; ++index)
          {
            if ((int) this.objects[index].objectID == this.objects[selectedObject].flag && this.objects[index].type == ObjectType.Flag)
            {
              if (!this.selectedObjects.Contains(index))
              {
                this.selectedObjects.Add(index);
                break;
              }
              break;
            }
          }
        }
      }
      this.selectedObjects.Sort();
      this.selectedObjects.Reverse();
      for (int index = 0; index < this.selectedObjects.Count<int>(); ++index)
      {
        if (this.objects[this.selectedObjects[index]].type == ObjectType.Flag)
        {
          string name = this.objects[this.selectedObjects[index]].name;
          if (name == "Jiggy Flag")
            this.usedJiggyFlags.Remove((int) this.objects[this.selectedObjects[index]].objectID);
          if (name == "Mumbo Token Flag")
            this.usedMTFlags.Remove((int) this.objects[this.selectedObjects[index]].objectID);
          if (name == "Empty Honeycomb Flag")
            this.usedHCFlags.Remove((int) this.objects[this.selectedObjects[index]].objectID);
        }
        this.objects.RemoveAt(this.selectedObjects[index]);
        GL.DeleteLists(this.objectsDList[this.selectedObjects[index]], 1);
        this.objectsDList.RemoveAt(this.selectedObjects[index]);
        GL.DeleteLists(this.objectsPickingDList[this.selectedObjects[index]], 1);
        this.objectsPickingDList.RemoveAt(this.selectedObjects[index]);
      }
      for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
      {
        this.structs.RemoveAt(this.selectedStructs[index]);
        GL.DeleteLists(this.structsDList[this.selectedStructs[index]], 1);
        this.structsDList.RemoveAt(this.selectedStructs[index]);
        GL.DeleteLists(this.structsPickingDList[this.selectedStructs[index]], 1);
        this.structsPickingDList.RemoveAt(this.selectedStructs[index]);
      }
      if (this.selectedCam != -1)
      {
        this.cameras.RemoveAt(this.selectedCam);
        this.cameraDList.RemoveAt(this.selectedCam);
      }
      for (int index = 0; index < this.paths.Count<BKPath>(); ++index)
      {
        if (this.selectedObjects.Contains(this.paths[index].pathObject))
          this.paths[index].pathObject = -1;
        foreach (int selectedObject in this.selectedObjects)
        {
          if (this.paths[index].pathObject > selectedObject)
            --this.paths[index].pathObject;
        }
      }
      this.selectedObjects.Clear();
      this.selectedStructs.Clear();
      this.selectedCam = -1;
      this.PairObjectsWithFlags();
    }

    public void UpdateScaleForSelectedObjects(short s)
    {
      for (int index = 0; index < this.selectedObjects.Count<int>(); ++index)
      {
        int selectedObject = this.selectedObjects[index];
        if (this.objects[selectedObject].radius == (ushort) 0)
        {
          this.objects[selectedObject].size += s;
          if (this.objects[selectedObject].size < (short) 10)
            this.objects[selectedObject].size = (short) 10;
          if (this.objects[selectedObject].size > (short) 800)
            this.objects[selectedObject].size = (short) 800;
        }
        else
        {
          this.objects[selectedObject].radius += (ushort) s;
          if (this.objects[selectedObject].radius < (ushort) 10)
            this.objects[selectedObject].radius = (ushort) 10;
          if (this.objects[selectedObject].radius > (ushort) 511)
            this.objects[selectedObject].radius = (ushort) 511;
        }
      }
      for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
      {
        int selectedStruct = this.selectedStructs[index];
        if (this.structs[selectedStruct].radius == (ushort) 0)
        {
          this.structs[selectedStruct].size += s;
          if (this.structs[selectedStruct].size < (short) 10)
            this.structs[selectedStruct].size = (short) 10;
          if (this.structs[selectedStruct].size > (short) 800)
            this.structs[selectedStruct].size = (short) 800;
        }
        else
        {
          this.structs[selectedStruct].radius += (ushort) s;
          if (this.structs[selectedStruct].radius < (ushort) 10)
            this.structs[selectedStruct].radius = (ushort) 10;
          if (this.structs[selectedStruct].radius > (ushort) 511)
            this.structs[selectedStruct].radius = (ushort) 511;
        }
      }
    }

    public void UpdateRotateForSelectedObjects(short r)
    {
      for (int index = 0; index < this.selectedObjects.Count<int>(); ++index)
      {
        int selectedObject = this.selectedObjects[index];
        this.objects[selectedObject].rot += r;
        if (this.objects[selectedObject].rot < (short) 0)
          this.objects[selectedObject].rot = (short) 360;
        if (this.objects[selectedObject].rot > (short) 360)
          this.objects[selectedObject].rot = (short) 0;
      }
      for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
      {
        int selectedStruct = this.selectedStructs[index];
        this.structs[selectedStruct].rot += r;
        if (this.structs[selectedStruct].rot < (short) 0)
          this.structs[selectedStruct].rot = (short) 360;
        if (this.structs[selectedStruct].rot > (short) 360)
          this.structs[selectedStruct].rot = (short) 0;
      }
    }

    public void DuplicateSelectedObjects(short x, short y, short z)
    {
      short[] forSelectedObjects = this.GetCenterPointForSelectedObjects(x, y, z, Mode.Object);
      for (int index = 0; index < this.selectedObjects.Count<int>(); ++index)
      {
        int selectedObject = this.selectedObjects[index];
        ObjectData o = new ObjectData(this.objects[selectedObject].objectID, 0, (short) ((int) this.objects[selectedObject].locX + (int) forSelectedObjects[0]), (short) ((int) this.objects[selectedObject].locY + (int) forSelectedObjects[1]), (short) ((int) this.objects[selectedObject].locZ + (int) forSelectedObjects[2]), this.objects[selectedObject].rot, this.objects[selectedObject].size, this.objects[selectedObject].specialScript);
        if (o.type == ObjectType.Normal)
          ObjectDB.FillObjectDetails(ref o);
        this.objects.Add(o);
      }
      for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
      {
        int selectedStruct = this.selectedStructs[index];
        ObjectData o = new ObjectData(this.structs[selectedStruct].objectID, 0, (short) ((int) this.structs[selectedStruct].locX + (int) forSelectedObjects[0]), (short) ((int) this.structs[selectedStruct].locY + (int) forSelectedObjects[1]), (short) ((int) this.structs[selectedStruct].locZ + (int) forSelectedObjects[2]), this.structs[selectedStruct].rot, this.structs[selectedStruct].size, this.structs[selectedStruct].specialScript);
        ObjectDB.FillObjectDetails(ref o);
        this.structs.Add(o);
      }
      this.RenderStructs();
      this.RenderObjects();
    }

    private short[] GetCenterPointForSelectedObjects(short x, short y, short z, Mode mode)
    {
      int[] numArray1 = new int[3]
      {
        int.MaxValue,
        int.MaxValue,
        int.MaxValue
      };
      int[] numArray2 = new int[3]
      {
        int.MinValue,
        int.MinValue,
        int.MinValue
      };
      int[] numArray3 = new int[3];
      if (mode == Mode.Object)
      {
        for (int index = 0; index < this.selectedObjects.Count<int>(); ++index)
        {
          numArray2[0] = (int) this.objects[this.selectedObjects[index]].locX > numArray2[0] ? (int) this.objects[this.selectedObjects[index]].locX : numArray2[0];
          numArray2[1] = (int) this.objects[this.selectedObjects[index]].locY > numArray2[1] ? (int) this.objects[this.selectedObjects[index]].locY : numArray2[1];
          numArray2[2] = (int) this.objects[this.selectedObjects[index]].locZ > numArray2[2] ? (int) this.objects[this.selectedObjects[index]].locZ : numArray2[2];
          numArray1[0] = (int) this.objects[this.selectedObjects[index]].locX < numArray1[0] ? (int) this.objects[this.selectedObjects[index]].locX : numArray1[0];
          numArray1[1] = (int) this.objects[this.selectedObjects[index]].locY < numArray1[1] ? (int) this.objects[this.selectedObjects[index]].locY : numArray1[1];
          numArray1[2] = (int) this.objects[this.selectedObjects[index]].locZ < numArray1[2] ? (int) this.objects[this.selectedObjects[index]].locZ : numArray1[2];
        }
        for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
        {
          numArray2[0] = (int) this.structs[this.selectedStructs[index]].locX > numArray2[0] ? (int) this.structs[this.selectedStructs[index]].locX : numArray2[0];
          numArray2[1] = (int) this.structs[this.selectedStructs[index]].locY > numArray2[1] ? (int) this.structs[this.selectedStructs[index]].locY : numArray2[1];
          numArray2[2] = (int) this.structs[this.selectedStructs[index]].locZ > numArray2[2] ? (int) this.structs[this.selectedStructs[index]].locZ : numArray2[2];
          numArray1[0] = (int) this.structs[this.selectedStructs[index]].locX < numArray1[0] ? (int) this.structs[this.selectedStructs[index]].locX : numArray1[0];
          numArray1[1] = (int) this.structs[this.selectedStructs[index]].locY < numArray1[1] ? (int) this.structs[this.selectedStructs[index]].locY : numArray1[1];
          numArray1[2] = (int) this.structs[this.selectedStructs[index]].locZ < numArray1[2] ? (int) this.structs[this.selectedStructs[index]].locZ : numArray1[2];
        }
      }
      if (mode == Mode.Path)
      {
        for (int index = 0; index < this.selectedNodes.Count<int>(); ++index)
        {
          ObjectData node = this.paths[this.selectedPath].nodes[this.selectedNodes[index]];
          numArray2[0] = (int) node.locX > numArray2[0] ? (int) node.locX : numArray2[0];
          numArray2[1] = (int) node.locY > numArray2[1] ? (int) node.locY : numArray2[1];
          numArray2[2] = (int) node.locZ > numArray2[2] ? (int) node.locZ : numArray2[2];
          numArray1[0] = (int) node.locX < numArray1[0] ? (int) node.locX : numArray1[0];
          numArray1[1] = (int) node.locY < numArray1[1] ? (int) node.locY : numArray1[1];
          numArray1[2] = (int) node.locZ < numArray1[2] ? (int) node.locZ : numArray1[2];
        }
        for (int index = 0; index < this.selectedSNodes.Count<int>(); ++index)
        {
          ObjectData node = this.paths[this.selectedPath].nodes[this.selectedSNodes[index]];
          numArray2[0] = (int) node.locX > numArray2[0] ? (int) node.locX : numArray2[0];
          numArray2[1] = (int) node.locY > numArray2[1] ? (int) node.locY : numArray2[1];
          numArray2[2] = (int) node.locZ > numArray2[2] ? (int) node.locZ : numArray2[2];
          numArray1[0] = (int) node.locX < numArray1[0] ? (int) node.locX : numArray1[0];
          numArray1[1] = (int) node.locY < numArray1[1] ? (int) node.locY : numArray1[1];
          numArray1[2] = (int) node.locZ < numArray1[2] ? (int) node.locZ : numArray1[2];
        }
      }
      for (int index = 0; index < 3; ++index)
        numArray3[index] = (numArray2[index] + numArray1[index]) / 2;
      return new short[3]
      {
        (short) ((int) x - numArray3[0]),
        (short) ((int) y - numArray3[1]),
        (short) ((int) z - numArray3[2])
      };
    }

    public void UpdateLocationForSelectedObjects(
      short x,
      short y,
      short z,
      bool ux,
      bool uy,
      bool uz,
      short yOffset)
    {
      short[] forSelectedObjects = this.GetCenterPointForSelectedObjects(x, y, z, Mode.Object);
      this.selectedObjects.Sort();
      this.selectedObjects.Reverse();
      this.selectedStructs.Sort();
      this.selectedStructs.Reverse();
      for (int index1 = 0; index1 < this.selectedObjects.Count<int>(); ++index1)
      {
        string[] strArray = new string[3]
        {
          "Jiggy",
          "Mumbo Token",
          "Empty Honeycomb"
        };
        foreach (string str in strArray)
        {
          if (this.objects[this.selectedObjects[index1]].name == str && this.objects[this.selectedObjects[index1]].flag != -1)
          {
            for (int index2 = 0; index2 < this.objects.Count; ++index2)
            {
              if (this.objects[index2].name.Contains(str + " Flag") && (int) this.objects[index2].objectID == this.objects[this.selectedObjects[index1]].flag && !this.selectedObjects.Contains(index2))
              {
                if (ux)
                  this.objects[index2].locX += forSelectedObjects[0];
                if (uy)
                  this.objects[index2].locY += (short) ((int) forSelectedObjects[1] + (int) yOffset);
                if (uz)
                  this.objects[index2].locZ += forSelectedObjects[2];
              }
            }
          }
        }
        int selectedObject = this.selectedObjects[index1];
        if (ux)
          this.objects[selectedObject].locX += forSelectedObjects[0];
        if (uy)
          this.objects[selectedObject].locY += (short) ((int) forSelectedObjects[1] + (int) yOffset);
        if (uz)
          this.objects[selectedObject].locZ += forSelectedObjects[2];
      }
      for (int index = 0; index < this.selectedStructs.Count<int>(); ++index)
      {
        int selectedStruct = this.selectedStructs[index];
        if (ux)
          this.structs[selectedStruct].locX += forSelectedObjects[0];
        if (uy)
          this.structs[selectedStruct].locY += (short) ((int) forSelectedObjects[1] + (int) yOffset);
        if (uz)
          this.structs[selectedStruct].locZ += forSelectedObjects[2];
      }
      if (this.selectedCam == -1)
        return;
      if (ux)
        this.cameras[this.selectedCam].x = (float) x;
      if (uy)
        this.cameras[this.selectedCam].y = (float) ((int) y + (int) yOffset);
      if (!uz)
        return;
      this.cameras[this.selectedCam].z = (float) z;
    }
  }
}
