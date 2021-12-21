// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BBXML
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using NLog;
using System;
using System.Collections.Generic;
using System.Xml;

namespace WumbasWigwam
{
  public class BBXML
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();

    public static List<SetupFile> ReadSetupsXML()
    {
      List<SetupFile> setupFileList = new List<SetupFile>();
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(".\\resources\\setups.xml");
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.Name == "setup")
          {
            string name = xmlTextReader.GetAttribute("name") == null ? "" : xmlTextReader.GetAttribute("name");
            int pointer = xmlTextReader.GetAttribute("pointer") == null ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("pointer"), 16);
            int sceneID = xmlTextReader.GetAttribute("sceneID") == null ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("sceneID"), 16);
            int modelAPointer = xmlTextReader.GetAttribute("modelAPointer") == null || xmlTextReader.GetAttribute("modelAPointer") == "" ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("modelAPointer"), 16);
            int modelBPointer = xmlTextReader.GetAttribute("modelBPointer") == null || xmlTextReader.GetAttribute("modelBPointer") == "" ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("modelBPointer"), 16);
            setupFileList.Add(new SetupFile(name, pointer, sceneID, modelAPointer, modelBPointer));
          }
        }
      }
      catch (Exception ex)
      {
        BBXML.logger.Log(NLog.LogLevel.Error, "Failed to open or read .\\resources\\setups.xml");
        BBXML.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      return setupFileList;
    }

    public static List<AnimationFile> readAnimationsXML(bool complete = false)
    {
      List<AnimationFile> animationFileList = new List<AnimationFile>();
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(".\\resources\\animations.xml");
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.Name == "animation")
          {
            string name_ = xmlTextReader.GetAttribute("name") == null ? "" : xmlTextReader.GetAttribute("name");
            int id_ = xmlTextReader.GetAttribute("id") == null ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("id"), 16);
            int offset_ = xmlTextReader.GetAttribute("offset") == null || xmlTextReader.GetAttribute("offset") == "" ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("offset"), 16);
            string[] strArray = (xmlTextReader.GetAttribute("modelpointer") == null ? "0" : xmlTextReader.GetAttribute("modelpointer")).Split(',');
            List<int> modelPointers_ = new List<int>();
            foreach (string str in strArray)
              modelPointers_.Add(Convert.ToInt32(str, 16));
            AnimationFile animationFile = new AnimationFile(id_, offset_, name_, modelPointers_);
            if ((uint) offset_ > 0U | complete && name_ != "X")
              animationFileList.Add(animationFile);
          }
        }
      }
      catch (Exception ex)
      {
        BBXML.logger.Log(NLog.LogLevel.Error, "Failed to open or read .\\resources\\animations.xml");
        BBXML.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      return animationFileList;
    }

    public static void readObjectsXML(
      ref List<ObjectData> objectList,
      ref List<ObjectData> structList)
    {
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(".\\resources\\objects.xml");
        while (xmlTextReader.Read())
        {
          string name = xmlTextReader.Name;
          ObjectData objectData = new ObjectData((short) 0, 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0);
          if (name == "object" || name == "struct")
          {
            string name_ = xmlTextReader.GetAttribute("name") == null ? "" : xmlTextReader.GetAttribute("name");
            short objectID_ = xmlTextReader.GetAttribute("id") == null ? (short) 0 : Convert.ToInt16(xmlTextReader.GetAttribute("id"), 16);
            short num1 = xmlTextReader.GetAttribute("script") == null ? (short) 0 : Convert.ToInt16(xmlTextReader.GetAttribute("script"), 16);
            int pointer_ = xmlTextReader.GetAttribute("pointer") == null ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("pointer"), 16);
            int pointer2_ = xmlTextReader.GetAttribute("pointer2") == null ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("pointer2"), 16);
            string modelfile_ = xmlTextReader.GetAttribute("modelfile") == null ? "" : xmlTextReader.GetAttribute("modelfile");
            string modelfile2_ = xmlTextReader.GetAttribute("modelfile2") == null ? "" : xmlTextReader.GetAttribute("modelfile2");
            short num2 = xmlTextReader.GetAttribute("cameraID") == null ? (short) -1 : Convert.ToInt16(xmlTextReader.GetAttribute("cameraID"), 16);
            short num3 = xmlTextReader.GetAttribute("jiggyID") == null ? (short) -1 : Convert.ToInt16(xmlTextReader.GetAttribute("jiggyID"), 16);
            objectData = num1 != (short) 19208 ? new ObjectData(name_, objectID_, num1, pointer_, pointer2_, modelfile_, modelfile2_, (int) num2, (int) num3) : new ObjectData(objectID_, 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0, num1);
          }
          if (!(name == "object"))
          {
            if (name == "struct")
              structList.Add(objectData);
          }
          else
            objectList.Add(objectData);
        }
      }
      catch (Exception ex)
      {
        BBXML.logger.Log(NLog.LogLevel.Error, "Failed to open or read .\\resources\\objects.xml");
        BBXML.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
    }

    public static List<Sprite> ReadSpriteXML()
    {
      List<Sprite> spriteList = new List<Sprite>();
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(".\\resources\\sprites.xml");
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.Name == "sprite")
          {
            string name_ = xmlTextReader.GetAttribute("name") == "" ? "" : xmlTextReader.GetAttribute("name");
            int id_ = xmlTextReader.GetAttribute("id") == "" ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("id"), 16);
            int pointer_ = xmlTextReader.GetAttribute("pointer") == "" ? 0 : Convert.ToInt32(xmlTextReader.GetAttribute("pointer"), 16);
            bool compressed_ = xmlTextReader.GetAttribute("compressed") == "" || xmlTextReader.GetAttribute("compressed") == null || Convert.ToBoolean(xmlTextReader.GetAttribute("compressed"));
            if (name_ != "X")
            {
              Sprite sprite = new Sprite(id_, name_, pointer_, compressed_);
              spriteList.Add(sprite);
            }
          }
        }
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        BBXML.logger.Log(NLog.LogLevel.Error, "Failed to open or read .\\resources\\sprites.xml");
        BBXML.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      return spriteList;
    }
  }
}
