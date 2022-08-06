#  温度自动采集系统 V1.0.0  
    本采集系统前端程序采用WPF结合MaterialDesign控件完成，后端采用keil控制GD32F130F8P6芯片完成对DS18B20传感器的温度采集，  
并将数据存储在AT24C32中，采集数据通过ModBus通讯协议完成数据上传并在前端程序显示。

|软件环境         |   版本 | 
| :----          |  :---- |
| Visual Studio  | Enterprise 16.9.4 |
| .NET FrameWork | 4.7.2 |
| Prism.Wpf      | 8.1.97 |
| MaterialDesign | Themes:4.5.0 Colors:2.0.6 |
| Microsoft.Practices.EnterpriseLibrary | 6.0.0.0 |
| SQLite.Core    | 1.0.116.0 |
| Dapper         | 2.0.123 |
| NModbus4       | 2.1.0 |


##  自编C#命名规范 V1.0  
1.字段、变量采用小驼峰法，属性、类名、方法名采用大驼峰法。  
2.只含有属性定义的简单类，以Kind结尾。  
3.枚举以Part结尾。  
4.List\<T>类型以Hub结尾。  
5.Dictionary\<T>类型以Shub结尾。  
6.ICollection\<T>类型以Collecter结尾。  