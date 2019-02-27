<template>
  <div>
    <el-table
      :height="tableHight"
      :row-class-name="tableRowClassName"
      :data="tableData"
    >
      <el-table-column
        :key="item.ID"
        v-for="item in columnData"
        :prop="item.prop"
        :label="item.label"
      ></el-table-column>
    </el-table>
  </div>
</template>

<style>
.el-table .green-row {
  background-color:#7CCD7C;
}
.el-table .gray-row {
  background-color:#838B8B
}
</style>

<script>
export default {
  data() {
    return {
      tableHight: window.innerHeight - 100 + "px",
      columnData: [
        { prop: "Date", label: "日期" },
        { prop: "Team1", label: "一组" },
        { prop: "Team2", label: "二组" },
        { prop: "Team3", label: "三组" },
        { prop: "Team4", label: "四组" },
        { prop: "Team5", label: "五组" }
      ],
      currentRow: null
    };
  },
  computed: {
    tableData() {
      let result = [
        {
          Date: "2019-02-23",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-03-02",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-03-09",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-03-16",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-03-23",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-03-30",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-04-13",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-04-20",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-04-27",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-05-04",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-05-11",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-05-18",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-05-25",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-06-01",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-06-15",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-06-22",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-06-29",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-07-06",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-07-13",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-07-20",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-07-27",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-08-03",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-08-10",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-08-17",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-08-24",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-08-31",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-09-07",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-09-21",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-09-28",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-10-12",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-10-19",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-10-26",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-11-02",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-11-09",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-11-16",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-11-23",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-11-30",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-12-07",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        },
        {
          Date: "2019-12-14",
          Team1: "刘威",
          Team2: "毛景杰",
          Team3: "雷星",
          Team4: "焦娇",
          Team5: "刘钰"
        },
        {
          Date: "2019-12-21",
          Team1: "李明",
          Team2: "苏莹",
          Team3: "李佩逸",
          Team4: "黄燕 ",
          Team5: " 李林玉 "
        },
        {
          Date: "2019-12-28",
          Team1: "钟文康",
          Team2: "詹张",
          Team3: "李庆强",
          Team4: "施礼斌",
          Team5: "马荣健"
        }
      ];
      return result;
    }
  },
  methods: {
    tableRowClassName({ row, rowIndex }) {
      let today = new Date();
      let currentDate = this.tableData.find(x => new Date(x.Date) >= today).Date;

      if (row.Date === currentDate) {
        return "green-row";
      }
      else if (row.Date < currentDate) {
        return "gray-row";
      }
      return "";
    }
  }
};
</script>
