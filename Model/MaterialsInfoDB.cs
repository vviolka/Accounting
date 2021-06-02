using System.Collections.Generic;
using System.Linq;
using Model.DBStructure;

namespace Model
{
    public class MaterialsInfoDB
    {
        private ApplicationContext ac;

        public MaterialsInfoDB()
        {
            ac = new ApplicationContext();
        }
        //добавить запись
        public int Add(MaterialsInfo materialsInfo)
        {
            using var dc = new ApplicationContext();
            dc.MaterialInfoes.Add(materialsInfo);
            dc.SaveChanges();
            return materialsInfo.Id;
        }
        //получить список материалов
        public List<MaterialsInfo> GetList()
        {
            using var dc = new ApplicationContext();
            var materialsInfo = dc.MaterialInfoes.ToList();

            return materialsInfo;
        }
        //получить названия материалов
        public List<string> GetNames()
        {
            using var dc = new ApplicationContext();
            var materialsInfo = dc.MaterialInfoes.Select(m => m.Name).ToList();

            return materialsInfo;
        }
        //изменить запись
        public void Edit(int id, MaterialsInfo oldMaterialsInfo)
        {
            using var dataContext = new ApplicationContext();
            var newMaterialsInfo = dataContext.MaterialInfoes.First(x => x.Id == id);
            newMaterialsInfo.Name = oldMaterialsInfo.Name;
            newMaterialsInfo.Unity = oldMaterialsInfo.Unity;
            newMaterialsInfo.Account = oldMaterialsInfo.Account;

            dataContext.SaveChanges();
        }
        //удалить запись
        public void Delete(MaterialsInfo materialsInfo)
        {
            using var dataContext = new ApplicationContext();
            var newMaterialsInfo = dataContext.MaterialInfoes.First(x => x.Id == materialsInfo.Id);
            dataContext.MaterialInfoes.Remove(newMaterialsInfo);
            dataContext.SaveChanges();
        }
    }
}