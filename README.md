#  温度自动采集系统 V1.0.0  
    本采集系统前端程序采用WPF结合MaterialDesign控件完成，后端采用keil控制GD32F130F8P6芯片完成对DS18B20传感器  
    的温度采集，并将数据存储在AT24C32中，采集数据通过ModBus通讯协议完成数据上传并在前端程序显示。  

## 程序环境  
|软件环境         |   版本 | 
| :----          |  :---- |
| Visual Studio Enterprise  | 16.9.4 |
| Keil MDK       | 5.29.0.0 |
| Altium Designer| 18.1.7 |
| .NET FrameWork | 4.7.2 |
| Prism.Wpf      | 8.1.97 |
| MaterialDesignThemes | 4.5.0 |
| MaterialDesignColors | 2.0.6 |
| Microsoft.Practices.EnterpriseLibrary | 6.0.0.0 |
| SQLite.Core    | 1.0.116.0 |
| Dapper         | 2.0.123 |
| NModbus4       | 2.1.0 |

## MaterialDesign控件风格约定
| 控件类型  | 默认风格名称 | 是否需要指定默认风格 |
| :----          |  :---- |  :---- |
| Window | MaterialDesignWindow | 是 |
| TextBlock | MaterialDesignTextBlock | 是 |
| ComboBox | MaterialDesignComboBox | 否 |
| ListBox | MaterialDesignChoiceChipPrimaryOutlineListBox | 是 |
| Button | MaterialDesignOutlinedLightButton | 是 |

##  程序设计规范 V1.0  
### 1. 视图模型类设计原则
- 各层之间交互关系如下:视图=>视图模型=>模型=>服务，视图模型层不直接使用服务层方法获取数据，模型层负责将服务层方法获取的数据  
  进行初步加工并交由视图模型层调用。 
- 对于程序错误只在程序中(即为Snackbar)提示出现错误，错误详细信息则记录在日志文件中，即视图模型层调用SnackbarMessageQueue
  完成错误提示，服务层调用LogService完成日志记录。  
- 服务层实现类都暴露公共的返回值为布尔类型的方法，方法参数中可以指定是否需要返回外部类型参数(使用out关键字).


##  命名规范 V1.0  
1.字段、变量采用小驼峰法，属性、类名、方法名采用大驼峰法。  
2.只含有属性定义的简单类型，使用结构体表示，并以Kind结尾。  
3.枚举类型以Part结尾。  
4.List\<T>类型以Hub结尾。  
5.Dictionary\<T>类型以Shub结尾。  
6.ICollection\<T>类型以Collecter结尾。  