
// (C) Copyright 2002-2005 by Autodesk, Inc. 
// 
// Permission to use, copy, modify, and distribute this software in 
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and 
// restricted rights notice below appear in all supporting 
// documentation. 
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF 
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE 
// UNINTERRUPTED OR ERROR FREE. 
// 
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer 
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii) 
// (Rights in Technical Data and Computer Software), as applicable. 

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using ClassLibrary1;
using X = Microsoft.Office.Interop.Excel;
using static ClassLibrary1.AviunUtils;

public class AutocadGetSetAttribute
{

    [CommandMethod("GTE", CommandFlags.UsePickSet)]
    public void VLL() // This method can have any name
    {
        List<BlockReferencePlacement> blockReferencePlacements = new List<BlockReferencePlacement>();
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        Database dwg = ed.Document.Database;
        Transaction trans = (Transaction)dwg.TransactionManager.StartTransaction();
        ///Filter
        TypedValue[] acTypValAr = {
            new TypedValue((int) DxfCode.Operator, "<and"),
            new TypedValue((int) DxfCode.Start, "LWPOLYLINE"),
            new TypedValue((int) DxfCode.LayerName, "S-Layout"),
            new TypedValue((int) DxfCode.Operator, "and>")};

        SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
        ///Selection
        PromptSelectionResult result = ed.GetSelection(acSelFtr);
        int selections = 0;
        if (result.Status == PromptStatus.OK)
        {
            SelectionSet selectionSet = result.Value;
            selections = selectionSet.Count;
            foreach (SelectedObject selectedObject in selectionSet)
            {
                BlockReferencePlacement block = new BlockReferencePlacement();
                int i = 0;
                // Check to make sure a valid SelectedObject object was returned
                if (selectedObject != null)
                {
                    ed.WriteMessage(selectedObject.ObjectId.ObjectClass.Name);
                    Polyline polyline = selectedObject.ObjectId.GetObject(OpenMode.ForRead) as Polyline;
                    for(int c=0;c<polyline.NumberOfVertices;c++)
                    {
                        block.xy = polyline.GetPoint2dAt(c);
                    }
                    blockReferencePlacements.Add(block);
                    ed.WriteMessage(polyline.NumberOfVertices.ToString());
                }

            }
        }
        else
        {
            ed.WriteMessage("Why khong chon Block nao het vay?");
        }

        trans.Commit();
        trans.Dispose();
        //List<BlockReferencePlacement> sorted = blockReferencePlacements.OrderBy(x => x.xyz.X).ToList();
        //List<BlockReferencePlacement> sorted2 = sorted.OrderByDescending(x => x.xyz.Y).ToList();
        //ed.WriteMessage(sorted2[1].content);
        //AddAnEnt(blockReferencePlacements);
    }

    //public void AddAnEnt(List<BlockReferencePlacement> t)
    //{

    //    // get the editor object so we can carry out some input 
    //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
    //    // if ok 
    //    // enter the name of the block 
    //    string blockName = "RebarAtt";
    //    Database dwg = ed.Document.Database;

    //    Transaction trans = (Transaction)dwg.TransactionManager.StartTransaction();
    //    try
    //    {
    //        BlockTableRecord newBlockDef = new BlockTableRecord();

    //        newBlockDef.Name = blockName;

    //        BlockTable blockTable = (BlockTable)trans.GetObject(dwg.BlockTableId, OpenMode.ForRead);


    //        if ((blockTable.Has(blockName) == false))
    //        {
    //            blockTable.UpgradeOpen();

    //            blockTable.Add(newBlockDef);

    //            trans.AddNewlyCreatedDBObject(newBlockDef, true);
    //            //Define a new block
    //            Circle circle1 = new Circle();
    //            circle1.Center = new Point3d(0, 0, 0);
    //            circle1.Radius = 1000;
    //            newBlockDef.AppendEntity(circle1);
    //            AttributeDefinition att1 = new AttributeDefinition();
    //            att1.Tag = "No";
    //            att1.Position = new Point3d(-1000, 0, 0);
    //            AttributeDefinition att2 = new AttributeDefinition();
    //            att2.Tag = "Diameter";
    //            att2.Position = new Point3d(0,0 , 0);
    //            AttributeDefinition att3 = new AttributeDefinition();
    //            att3.Tag = "Spacing";
    //            att3.Position = new Point3d(1000, 0, 0);
    //            AttributeDefinition att4 = new AttributeDefinition();
    //            att4.Tag = "Length";
    //            att4.Position = new Point3d(0, 1000, 0);
    //            AttributeDefinition att5 = new AttributeDefinition();
    //            att5.Tag = "Distance";
    //            att5.Position = new Point3d(0, -1000, 0);
    //            newBlockDef.AppendEntity(att1);
    //            newBlockDef.AppendEntity(att2);
    //            newBlockDef.AppendEntity(att3);
    //            newBlockDef.AppendEntity(att4);
    //            newBlockDef.AppendEntity(att5);
    //            trans.AddNewlyCreatedDBObject(circle1, true);
    //            trans.AddNewlyCreatedDBObject(att1, true);
    //            trans.AddNewlyCreatedDBObject(att2, true);
    //            trans.AddNewlyCreatedDBObject(att3, true);
    //            trans.AddNewlyCreatedDBObject(att4, true);
    //            trans.AddNewlyCreatedDBObject(att5, true);

    //            //Declare a BlockReference to place in current Space of Autocad

    //            BlockReference blockRef = new BlockReference(t[0].xyz, newBlockDef.ObjectId);

    //            BlockTableRecord curSpace = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);
    //            //Append Block Reference to this space
    //            curSpace.AppendEntity(blockRef);
    //            trans.AddNewlyCreatedDBObject(blockRef, true);
    //            //BlockTableRecord, get the blockname to read its Attribute Definition
    //            BlockTableRecord blockDef = blockTable[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;
    //            foreach (ObjectId id in blockDef)
    //            {
    //                AttributeDefinition attributeDefinition = id.GetObject(OpenMode.ForRead) as AttributeDefinition;
    //                if ((attributeDefinition != null) && (!attributeDefinition.Constant))
    //                {
    //                    switch (attributeDefinition.Tag)
    //                    {
    //                        case "No":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].numberofsteels;
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Diameter":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].diameter;
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Spacing":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].spacing;
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Length":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].length.ToString();
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Distance":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].distance.ToString();
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                    }

    //                }
    //            }
    //            // 34. If the code makes it here then all is ok. Commit the transaction by calling 
    //            // the Commit method 
    //            trans.Commit();
    //        }
    //        else
    //        {
    //            BlockTableRecord blockDef = blockTable[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;

    //            //Declare a BlockReference to place in current Space of Autocad

    //            BlockReference blockRef = new BlockReference(t[0].xyz, blockDef.ObjectId);

    //            BlockTableRecord curSpace = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);
    //            //Append Block Reference to this space
    //            curSpace.AppendEntity(blockRef);
    //            trans.AddNewlyCreatedDBObject(blockRef, true);
    //            //BlockTableRecord, get the blockname to read its Attribute Definition
    //            foreach (ObjectId id in blockDef)
    //            {
    //                AttributeDefinition attributeDefinition = id.GetObject(OpenMode.ForRead) as AttributeDefinition;
    //                if ((attributeDefinition != null) && (!attributeDefinition.Constant))
    //                {
    //                    switch (attributeDefinition.Tag)
    //                    {
    //                        case "No":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].numberofsteels;
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Diameter":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].diameter;
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Spacing":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].spacing;
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Length":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].length.ToString();
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                        case "Distance":
    //                            using (AttributeReference attRef = new AttributeReference())
    //                            {
    //                                attRef.SetAttributeFromBlock(attributeDefinition, blockRef.BlockTransform);
    //                                attRef.TextString = t[0].distance.ToString();
    //                                //Add the AttributeReference to the BlockReference
    //                                blockRef.AttributeCollection.AppendAttribute(attRef);
    //                                trans.AddNewlyCreatedDBObject(attRef, true);
    //                            }
    //                            break;
    //                    }

    //                }
    //            }
    //            // 34. If the code makes it here then all is ok. Commit the transaction by calling 
    //            // the Commit method 
    //            trans.Commit();
    //        }
    //    }
    //    catch (Autodesk.AutoCAD.Runtime.Exception ex)
    //    {
    //        ed.WriteMessage("Chon dung hoac it hon 6 Block thoi nha! " + ex.Message);
    //    }
    //    finally
    //    {
    //        trans.Dispose();
    //    }
    //}
}
