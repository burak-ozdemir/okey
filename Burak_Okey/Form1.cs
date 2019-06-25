using Okey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Burak_Okey
{
    public partial class Form1 : Form
    {
        const int FirstOkeyId = -1;
        const int SecondOkeyId = -2;
        const int FirstFakeOkeyId = -3;
        const int SecondFakeOkeyId = -4;
        const int Yellow = 0;
        const int Blue = 1;
        const int Black = 2;
        const int Red = 3;

        List<RummyTile> AllRummyTiles = new List<RummyTile>();
        List<BillardCue> BillardCues = new List<BillardCue>();
        RummyTile SelectorRummyTile = new RummyTile();
        RummyTile FirstOkey = new RummyTile();
        RummyTile SecondOkey = new RummyTile();
        RummyTile FirstFakeOkey = new RummyTile();
        RummyTile SecondFakeOkey = new RummyTile();
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            CreateRummyTiles();
            ChooseSelectorAndOkey();
            AddFakeOkeysInList();
            MixList();
            ShareRummyTiles();
            BillardCueInfo();


        }
        void CreateRummyTiles()
        {
            /*
             sarı    - 0  
             mavi    - 1 
             siyah   - 2
             kırmızı - 3
             */
            int idCounter = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        var rummyTile = new RummyTile
                        {
                            Id = idCounter,
                            Color = i,
                            Number = (j + 1),
                            SNumber = (j + 1).ToString()

                        };
                        AllRummyTiles.Add(rummyTile);
                        idCounter++;
                    }
                }
            }   

        }

        void ChooseSelectorAndOkey()
        {
          
            int selectorId = rnd.Next(0, 104);
            SelectorRummyTile = AllRummyTiles.Where(x => x.Id == selectorId).First();
            int okeyNumber = 0;
            if (SelectorRummyTile.Number == 13)
            {
                okeyNumber = 1;
            }
            else
            {
                okeyNumber = SelectorRummyTile.Number + 1; 
            }

            var okeyList = AllRummyTiles.Where(x => (x.Number == okeyNumber) && (x.Color == SelectorRummyTile.Color)).ToList();

            FirstOkey = new RummyTile
            {
                Id = okeyList[0].Id,
                Number = okeyNumber,
                SNumber = okeyNumber.ToString(),
                Color = SelectorRummyTile.Color,
                RummyTileSpecialId = FirstOkeyId
            };
            SecondOkey = new RummyTile
            {
                Id = okeyList[1].Id,
                Number = okeyNumber,
                SNumber = okeyNumber.ToString(),
                Color = SelectorRummyTile.Color,
                RummyTileSpecialId = SecondOkeyId
            };

         

            AllRummyTiles.Remove(okeyList[0]);
            AllRummyTiles.Remove(okeyList[1]);

            okeyList[0].RummyTileSpecialId = FirstOkeyId;
            okeyList[1].RummyTileSpecialId = SecondOkeyId;

            AllRummyTiles.Add(okeyList[0]);
            AllRummyTiles.Add(okeyList[1]);

            string scolor = "";
            if (SelectorRummyTile.Color == 0)
            {
               scolor = "sarı";
            }
            else if (SelectorRummyTile.Color == 1)
            {
                scolor = "mavi";
            }
            else if (SelectorRummyTile.Color == 2)
            {
                scolor = "siyah";
            }
            else
            {
                scolor = "kırmızı";
            }

            string color = "";
            if (FirstOkey.Color == 0)
            {
                color = "sarı";
            }
            else if (FirstOkey.Color == 1)
            {
                color = "mavi";
            }
            else if (FirstOkey.Color == 2)
            {
                color = "siyah";
            }
            else
            {
                color = "kırmızı";
            }

            label1.Text = string.Format("gösterge: {0} - renk :{1}",SelectorRummyTile.Number,scolor );
            label2.Text = string.Format("okey: {0} - renk :{1}", FirstOkey.Number, color);

        }
        void AddFakeOkeysInList()
        {
            FirstFakeOkey = new RummyTile
            {
                Id = 104,
                Number = FirstOkey.Number,
                SNumber = "*",
                Color = FirstOkey.Color,
                RummyTileSpecialId = FirstFakeOkeyId
            };
            SecondFakeOkey = new RummyTile
            {
                Id = 105,
                Number = FirstOkey.Number,
                SNumber = "*",
                Color = FirstOkey.Color,
                RummyTileSpecialId = SecondFakeOkeyId
            };
            AllRummyTiles.Add(SelectorRummyTile);
            AllRummyTiles.Add(SelectorRummyTile);
        }
        void MixList()
        {
            for (int i = 0; i < 5; i++)
            {
                AllRummyTiles = Shuffle(AllRummyTiles);
            }     
        }
        List<RummyTile> Shuffle (List<RummyTile> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                RummyTile value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        return list;
        }
        void ShareRummyTiles()
        {
            int playerOne = rnd.Next(4);
            int rummyTileCounter = 0;
            for (int i = 0; i < 4; i++)
            {
                BillardCue billardCue = new BillardCue();
                billardCue.BillardCueId = i;
                billardCue.TilesOnBoard = AllRummyTiles.GetRange(rummyTileCounter, 14);
                rummyTileCounter = rummyTileCounter + 14;
                if (i == playerOne )
                {
                    billardCue.TilesOnBoard.Add(AllRummyTiles[rummyTileCounter]);
                    rummyTileCounter = rummyTileCounter + 1;
                }
                BillardCues.Add(billardCue);
            }

        }
      
        bool CheckItemIsInList(List<RummyTile> list, RummyTile rummyTile)
        {
            var control = list.Contains(rummyTile);
            return control;
        }

        List<RummyTile> Analise(List<RummyTile> list)
        {
            List<List<RummyTile>> possibleMoveList = new List<List<RummyTile>>();
            var colorGroups = from rum in list
                              orderby rum.Number
                              group rum by rum.Color into ColorGroup
                              select ColorGroup;
            colorGroups = colorGroups.OrderBy(x => x.Key);
            var isHasOkeyStones = list.Where(x => (x.RummyTileSpecialId == -1) || (x.RummyTileSpecialId == -2)).ToList();
            foreach (var item in colorGroups)
            {
                var sameColorList = item.ToList();
                List<RummyTile> canUsable = new List<RummyTile>();
                var ifHasOkeyStones = list.Where(x => (x.RummyTileSpecialId == -1) || (x.RummyTileSpecialId == -2)).ToList();
                int hasOkeyStonesCount = ifHasOkeyStones.Count;
                int usedOkeyStonesCounter = 0;
                for (int i = 0; i < sameColorList.Count; i++)
                {
                    if((i + 1) == sameColorList.Count)
                    {
                        if (canUsable.Count >= 3)
                        {
                            possibleMoveList.Add(canUsable);
                        }
                        else if(canUsable.Count == 2 && hasOkeyStonesCount > usedOkeyStonesCounter)
                        {
                            canUsable.Add(ifHasOkeyStones[0]);
                            possibleMoveList.Add(canUsable);
                        }
                        canUsable = new List<RummyTile>();
                        break;
                    }
                    if(sameColorList[i].Number + 1 == sameColorList[i + 1].Number)
                    {
                        if (! CheckItemIsInList(canUsable,sameColorList[i]))
                        {
                            if(ifHasOkeyStones.Count != 0 && hasOkeyStonesCount > usedOkeyStonesCounter)
                            {
                                if (sameColorList[i].Color == ifHasOkeyStones[0].Color
                                && sameColorList[i].Number == ifHasOkeyStones[0].Number)
                                {
                                    canUsable.Add(sameColorList[i]);
                                    usedOkeyStonesCounter++;
                                }
                                else if (sameColorList[i] != ifHasOkeyStones[0])
                                {
                                    canUsable.Add(sameColorList[i]);
                                }
                            }
                            else
                            {
                                if (ifHasOkeyStones.Count != 0 
                                    && sameColorList[i].Color != ifHasOkeyStones[0].Color
                                   && sameColorList[i].Number != ifHasOkeyStones[0].Number)
                                {
                                    canUsable.Add(sameColorList[i]);
                                }
                                else if (ifHasOkeyStones.Count == 0)
                                {
                                    canUsable.Add(sameColorList[i]);
                                }
                            }
                               
                        }
                        if (!CheckItemIsInList(canUsable, sameColorList[i + 1]))
                        {
                            if (ifHasOkeyStones.Count != 0 && hasOkeyStonesCount > usedOkeyStonesCounter)
                            {
                                if (sameColorList[i + 1].Color == ifHasOkeyStones[0].Color
                                && sameColorList[i + 1].Number == ifHasOkeyStones[0].Number)
                                {
                                canUsable.Add(sameColorList[i + 1]);
                                usedOkeyStonesCounter++;
                                }
                                else if (sameColorList[i + 1] != ifHasOkeyStones[0])
                                {
                                canUsable.Add(sameColorList[i +1]);
                                }
                            }
                            else
                            {
                                if (ifHasOkeyStones.Count != 0
                                    && sameColorList[i + 1].Color != ifHasOkeyStones[0].Color
                                    && sameColorList[i + 1].Number != ifHasOkeyStones[0].Number)
                                {
                                    canUsable.Add(sameColorList[i + 1]);
                                }
                                else if (ifHasOkeyStones.Count == 0)
                                {
                                    canUsable.Add(sameColorList[i + 1]);
                                }
                            }
                        }
                    }
                    else if(sameColorList[i].Number + 2 == sameColorList[i + 1 ].Number
                        && ifHasOkeyStones.Count != 0
                        && ifHasOkeyStones[0] != sameColorList[i]
                        && ifHasOkeyStones[0] != sameColorList[i + 1]
                        && hasOkeyStonesCount > usedOkeyStonesCounter
                        )
                        {
                            if (!CheckItemIsInList(canUsable, sameColorList[i]))
                            {
                                canUsable.Add(sameColorList[i]);
                            }
                            if (!CheckItemIsInList(canUsable, ifHasOkeyStones[0]))
                            {
                            canUsable.Add(ifHasOkeyStones[0]);
                            usedOkeyStonesCounter++;
                            }
                            if (!CheckItemIsInList(canUsable, sameColorList[i + 1]))
                            {
                                canUsable.Add(sameColorList[i + 1]);
                            }
                    }
                    else
                    {
                        if(canUsable.Count >= 3)
                        {
                            possibleMoveList.Add(canUsable);
                        }
                        canUsable = new List<RummyTile>();
                    }
                }
            }

            //renklere göre sıralı olanları aldıktan sonra aynı numaralı farklı renkte olan grupları almak istiyoruz
            //ancak önce elimizdeki okeyi burada iyi kullanabilirmiyiz kontrol edelim
           
            var maxItemInUsableList = new List<RummyTile>();
            if(possibleMoveList.Count() != 0 )
            {
                maxItemInUsableList = possibleMoveList.OrderByDescending(x => x.Count()).First();
            }
            //Eğer kullanılması muhtemel hamleler listemizde 4 lü veya 5 li perler varsa diğer duruma hiç bakılmaz
            if (maxItemInUsableList.Count < 4 && isHasOkeyStones.Count != 0)
            {
                int groupMemberCount = 1;
                if (maxItemInUsableList.Count > 1)
                {
                    groupMemberCount = maxItemInUsableList.Count;
                }

                var numberGroup = from rum in list
                                  orderby rum.Number
                                  group rum by rum.Number
                                             into NumberGroup
                                  select NumberGroup;
                var isThereFourtPerWithoutOkey = false;
                var maxGroup = numberGroup.OrderByDescending(x => x.Count());
                foreach (var item in maxGroup)
                {
                    var subList = item.ToList();
                    var rummyTile = subList[0];
                    var usingTempTiles = new List<RummyTile>();
                    usingTempTiles.Add(rummyTile);
                    for (int i = 1; i < subList.Count; i++)
                    {
                        var query = usingTempTiles.Where(x => x.Color == subList[i].Color).ToList();
                        if (query.Count == 0)
                        {
                            usingTempTiles.Add(subList[i]);
                        }
                    }
                    if (usingTempTiles.Count == 3)
                    {
                        usingTempTiles.Add(isHasOkeyStones[0]);
                        possibleMoveList.Add(usingTempTiles);
                    }
                    else if (usingTempTiles.Count > 3)
                    {
                        isThereFourtPerWithoutOkey = true;
                        list = DeleteFromList(list, usingTempTiles);
                        usingTempTiles = new List<RummyTile>();
                        /*
                        eğer okey kullanmadan 4 lü per yapabiliyorsak perleri listeden çıkartıp tekrar aynı fonksiyonu çağırırız.    
                     */
                    }
                }
                if (isThereFourtPerWithoutOkey)
                {
                    list = Analise(list);
                }
            }

            var haveMaxItemList = new List<RummyTile>();
            if (possibleMoveList.Count() != 0 && isHasOkeyStones.Count() > 0)
            {
                haveMaxItemList = possibleMoveList.OrderByDescending(x => x.Count()).First();
                list = DeleteFromList(list, haveMaxItemList);
            }
            else if (possibleMoveList.Count() != 0 && isHasOkeyStones.Count() == 0)
            {
                foreach (var item in possibleMoveList)
                {
                    list = DeleteFromList(list, item);
                }
            }

   
            

            var numberGroup2 = from rum in list
                              orderby rum.Number
                              group rum by rum.Number
                                            into NumberGroup
                              where NumberGroup.Count() > 2
                              select NumberGroup;

            var usingTiles = new List<RummyTile>();

            foreach (var item in numberGroup2)
            {
                var subList = item.ToList();
                var rummyTile = subList[0];
                var usingTempTiles = new List<RummyTile>();
                usingTempTiles.Add(rummyTile);
                for (int i = 1; i < subList.Count; i++)
                {
                    var query = usingTempTiles.Where(x => x.Color == subList[i].Color).ToList();
                    if (query.Count == 0)
                    {
                        usingTempTiles.Add(subList[i]);
                    }
                }
                if (usingTempTiles.Count >= 3)
                {
                    usingTiles.AddRange(usingTempTiles);
                }
            }
            list = DeleteFromList(list,usingTiles);
            return list;
        }
        List<RummyTile> DeleteFromList(List<RummyTile> mainList, List<RummyTile> deleteList)
        {
            for (int i = 0; i < deleteList.Count; i++)
            {
                var query = mainList.Remove(mainList.Where(x => x.Id == deleteList[i].Id).First());
            }
            return mainList;
        }
        void ClearListBoxes(List<ListBox> lists)
        {
            foreach (var item in lists)
            {
                item.Items.Clear();
            }
        }
        void BillardCueInfo()
        {
            
            List<ListBox> lists = new List<ListBox>();
            lists.Add(listBox1);
            lists.Add(listBox2);
            lists.Add(listBox3);
            lists.Add(listBox4);
            ClearListBoxes(lists);
            for (int i = 0; i < 4; i++)
            {
                BillardCues[i].TilesOnBoard = BillardCues[i].TilesOnBoard.OrderBy(x => x.Number).ToList();
                BillardCues[i].TilesOnBoard = BillardCues[i].TilesOnBoard.OrderBy(x => x.Color).ToList();
                for (int j = 0; j < BillardCues[i].TilesOnBoard.Count; j++)
                {
                    string color = "";
                    if (BillardCues[i].TilesOnBoard[j].Color == 0)
                    {
                        color = "sarı";
                    }
                    else if (BillardCues[i].TilesOnBoard[j].Color == 1)
                    {
                        color = "mavi";
                    }
                    else if (BillardCues[i].TilesOnBoard[j].Color == 2)
                    {
                        color = "siyah"; 
                    }
                    else
                    {
                        color = "kırmızı"; 
                    }
                    string element = string.Format("{0}--{1}",color, BillardCues[i].TilesOnBoard[j].SNumber);
                    lists[i].Items.Add(element);

                }
            }
        }

        
        int GoDouble(List<RummyTile> list)
        {
            List<RummyTile> usingTiles = new List<RummyTile>();
            var colorGroup = list.GroupBy(x => x.Color).ToList();
            foreach (var item in colorGroup)
            {
                var numberGroup = item.GroupBy(x => x.Number).ToList();
                foreach (var subItem in numberGroup)
                {
                    var itemList = subItem.ToList();
                    if(itemList.Count > 1)
                    {
                        usingTiles.AddRange(itemList);
                    }
                }
            }

            return usingTiles.Count();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            var bOne = new List<RummyTile>(); bOne.AddRange(BillardCues[0].TilesOnBoard);
            var bTwo = new List<RummyTile>(); bTwo.AddRange(BillardCues[1].TilesOnBoard);
            var bThree = new List<RummyTile>(); bThree.AddRange(BillardCues[2].TilesOnBoard);
            var bFour = new List<RummyTile>(); bFour.AddRange(BillardCues[3].TilesOnBoard);
            int first = BillardCues[0].TilesOnBoard.Count - Analise(bOne).Count;
            int second = BillardCues[1].TilesOnBoard.Count - Analise(bTwo).Count;
            int third = BillardCues[2].TilesOnBoard.Count - Analise(bThree).Count;
            int fourth = BillardCues[3].TilesOnBoard.Count - Analise(bFour).Count;
            int doubleFirst = GoDouble(BillardCues[0].TilesOnBoard);
            int doubleSecond = GoDouble(BillardCues[1].TilesOnBoard);
            int doubleThird = GoDouble(BillardCues[2].TilesOnBoard);
            int doubleFourth = GoDouble(BillardCues[3].TilesOnBoard);

            BillardCueOneNormal.Text = first.ToString();
            BillardCueTwoNormal.Text = second.ToString();
            BillardCueThreeNormal.Text = third.ToString();
            BillardCueFourNormal.Text = fourth.ToString();

            BillardCueOneDouble.Text = doubleFirst.ToString();
            BillardCueTwoDouble.Text = doubleSecond.ToString();
            BillardCueThreeDouble.Text = doubleThird.ToString();
            BillardCueFourDouble.Text = doubleFourth.ToString();
        }

    }
}