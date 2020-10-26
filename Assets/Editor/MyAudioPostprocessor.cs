using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MyAudioPostprocessor : AssetPostprocessor
{
   public void OnPreprocessAudio()
   {
      var audioImporter = assetImporter as AudioImporter;
      var audioSettings = new AudioImporterSampleSettings();
      audioImporter.preloadAudioData = true;
      audioImporter.loadInBackground = true;
      var info = new FileInfo(assetPath).Length/1024;
      if (info < 200)
      {
         audioSettings.loadType = AudioClipLoadType.DecompressOnLoad;
      }
      else if(info < 5000)
      {
         audioSettings.loadType = AudioClipLoadType.CompressedInMemory;
      }
      else
      {
         audioSettings.loadType = AudioClipLoadType.Streaming;
      }
      
   }
}
