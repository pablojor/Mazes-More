using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class ProgessManager
    {
        /// <summary>
        /// struct que representa los valores que contiene un guardado
        /// </summary>
        public struct Save
        {
            public int c, i, h;
        }
        /// <summary>
        /// progreso actual
        /// </summary>
        private Save _mySave;

        /// <summary>
        /// deserializa el progreso y se lo guarda
        /// </summary>
        public void ReadProgress()
        {
            JSONSave progress = JSONSave.FromJson();

            _mySave.c = progress.c;
            _mySave.i = progress.i;
            _mySave.h = progress.h;
        }

        /// <summary>
        /// </summary>
        /// <returns>progreso actual</returns>
        public Save GetProgess()
        {
            return _mySave;
        }

        /// <summary>
        /// actualiza el progreso
        /// </summary>
        /// <param name="newSave">nuevo progreso</param>
        public void UpdateProgess(Save newSave)
        {
            _mySave = newSave;
            JSONSave save = new JSONSave();

            save.c = newSave.c;
            save.i = newSave.i;
            save.h = newSave.h;

            save.ToJson();
        }
    }
}