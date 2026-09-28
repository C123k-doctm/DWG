using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeighaCad_Bll
{
    /// <summary>
    /// 定义一个命令角色。一般是一个接口，为所有的命令对象声明一个接口，规范将要进行的命令操作。Command
    /// </summary> 
    public interface Command
    {
        /**
         * 执行命令
         */
        void execute();

        /**
         * 撤销命令
         */
        void undo();

        /**
         * 清除命令
         */
        void clear();
    }
}
