// "use client";
// import { ITask, IUserInfo, IUserRights } from "@/types";
// import { Dialog, DialogContent } from "../ui/dialog";
// import {
//   Gantt,
//   Task,
//   EventOption,
//   StylingOption,
//   ViewMode,
//   DisplayOption,
// } from "gantt-task-react";
// import "gantt-task-react/dist/index.css";
// import { useState, useEffect, useCallback } from "react";
// import {
//   useParams,
//   usePathname,
//   useRouter,
//   useSearchParams,
// } from "next/navigation";
// import dayjs from "dayjs";
// import TaskModal from "../taskModal";
// import { GetRights, GetTasksByProjectId } from "@/requests";

// interface ITaskListHeader {
//   headerHeight: number;
//   rowWidth: string;
//   fontFamily: string;
//   fontSize: string;
// }

// const TaskListHeader: React.FC<ITaskListHeader> = () => {
//   return (
//     <div
//       className="dark:text-white border-b border-black/10"
//       style={{
//         width: "100%",
//         marginTop: "1.5rem",
//         display: "flex",
//         flexDirection: "row",
//         justifyContent: "space-around",
//         // borderBottom: "1px solid rgba(1,1,1,0.1)",
//       }}
//     >
//       <div>Название</div>
//       <div>Дата начала</div>
//       <div>Дата конца</div>
//     </div>
//   );
// };

// interface IToolTipContent{
//   task: Task,
//   fontSize: string,
//   fontFamily: string
// }

// const ToolTipContent: React.FC<IToolTipContent> = ({task}) => {
//   return(
//     <div className="bg-gray-100 border border-gray-200 h-20 w-50 p-4 rounded-lg flex flex-col">
//       <p className="m-0 p-0 text-[12px]">{task.name}</p>
//       <div>
//         <p className="m-0 p-0 text-[12px]">Прогресс: {task.progress}</p>
//       </div>
//     </div>
//   )
// }

// const ExampleTask: Task[] = [{
//     start: new Date(),
//         end: new Date(),
//         name: 'Пример',
//         id: 'Task 0',
//         type:'task',
//         progress: 50,
//         isDisabled: true,
//         styles: { progressColor: 'rgba(102, 153, 255, 01)', progressSelectedColor: '#ff9e0d' }
// }];


// interface ITaskDto{
//   task: ITask,
//   user: IUserInfo
// }

// export default function GanttChart() {
//   const [toFetch, setFetch] = useState<boolean>(false);
//   const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
//   const [rights, setRights] = useState<UserRights>();
//   const { id } = useParams<{ id: string }>();
//   const pathname = usePathname();
//   const router = useRouter();
//   const searchParams = useSearchParams();
//   const [tasks, setTasks] = useState<Task[]>([]);
//   const [newtasks, setnewTasks] = useState<ITask[]>([]);

//   useEffect(() => {
//     getTasks(id);
//     getRights();
//   }, [id])

//   useEffect(() => {
//     if (toFetch) {
//       getTasks(id);
//       setFetch(false);
//     }
//   }, [toFetch]);

//   const getRights = async () => {
//     const userId = localStorage.getItem("userId");
//     const response = await GetRights(id, userId!);
//     const data = response.data;
//     setRights(data);
//   };

//   const getTasks = async (id: string) => {
//     const response = await GetTasksByProjectId(id);
//     const data = response.data;

//     if(data.length <= 0) return;
//     setTasks([]);

//     data.map((taskDto : ITaskDto)  => {
//         const startDate = new Date(dayjs(taskDto.task.startDate).toString());
//         const finishDate = new Date(dayjs(taskDto.task.finishDate).toString());
//         setTasks(tasks => [...tasks, {
//             id: taskDto.task.id,
//             name: taskDto.task.name,
//             type: 'task',
//             progress: taskDto.task.progress,
//             start: startDate,
//             end: finishDate,
//             isDisabled: true,
//             styles: { progressColor: 'rgba(102, 153, 255, 0.1)', progressSelectedColor: '#248f24' }
//           }])
//     })
//   };

//   const handleOpenModal = useCallback(
//     (taskId?: string) => {
//       const params = new URLSearchParams(searchParams.toString());
//       if (taskId) params.set("task", taskId);
//       else params.delete("task");
//       return params.toString();
//     },
//     [searchParams]
//   );

//   return (
//     <div style={{marginLeft: "4rem"}}>
//         <Gantt
//         locale="ru"
//         tasks={tasks.length > 0 ? tasks : ExampleTask}
//         fontFamily="inherit"
//         TaskListHeader={TaskListHeader}
//         barBackgroundColor="rgb(102, 153, 255)"
//         barCornerRadius={12}
//         barFill={60}
//         ganttHeight={0}
//         TooltipContent={ToolTipContent}
//         listCellWidth=""
//         todayColor="#ff9933"
//         onClick={(task) =>
//           router.push(pathname + "?" + handleOpenModal(task.id))
//         }
//         />
//         <div>
//         <Dialog
//           open={searchParams.has("task")}
//         >
//           <DialogContent>
//             <div
//               style={{
//                 display: "flex",
//                 justifyContent: "center",
//                 alignSelf: "center",
//                 alignContent: "center",
//               }}
//             >
//               <TaskModal
//                 onClose={() => router.push(pathname + "?" + handleOpenModal())}
//                 setFetch={setFetch}
//                 rights={rights}
//               ></TaskModal>
//             </div>
//           </DialogContent>
//         </Dialog>
//       </div>
//     </div>
//   );
// }
