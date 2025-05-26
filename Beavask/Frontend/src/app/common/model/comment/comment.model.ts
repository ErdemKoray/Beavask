import { Task } from "../../services/task/taskModel/task.model";
import { User } from "../user.model";

export interface Comment{
    id:number,
    title:string,
    content:string,
    taskId:number,
    createdAt:Date,
    updateAt:Date,
    isActive:boolean  
    task?: Task;      // optional ilişkili task bilgisi
  user?: User;      // optional kullanıcı bilgisi
}