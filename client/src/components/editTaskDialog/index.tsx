"use client";
import { useState, useEffect, Dispatch, SetStateAction } from "react";
import { TaskDto, ITaskComment, IUser, UserRights, StageDto, StageType, UserDto, UpdateTaskDto } from "@/types";
import { Button } from "@/components/ui/button";
import { Select, SelectContent, SelectItem, SelectValue, SelectTrigger } from "@/components/ui/select";
import { Avatar } from "@/components/ui/avatar";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { X, Send } from "lucide-react";
import { useParams, useSearchParams } from "next/navigation";
import { cn } from "@/lib/utils";
import { Calendar as CalendarIcon } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import Editor from "@/components/editor";
import { HubConnectionBuilder, HubConnection, HubConnectionState } from "@microsoft/signalr";
import { Input } from "@/components/ui/input";
import { ScrollArea } from "../ui/scroll-area";
import { deleteTask, getStagesByProjectId, getTask, getUsersByProjectId, updateTask, updateTaskStage } from "@/requests";
import { format } from "date-fns";
import {StageMapperService} from "@/utils/stage-mapper.service";

interface Props {
    onClose: () => void;
    setFetch: Dispatch<SetStateAction<boolean>>;
    rights: UserRights | undefined;
}

const EditTaskDialog = ({ onClose, setFetch, rights }: Props) => {
    const params = useParams<{ id: string }>();
    const [isActive] = useState<boolean>(true);
    const searchParams = useSearchParams();
    const [currentTask, setTask] = useState<TaskDto>();
    const [responsibleUser, setResponsibleUser] = useState<UserDto>({ username: "Все" } as UserDto);
    const [users, setUsers] = useState<UserDto[]>([]);
    const [statusList, setStatusList] = useState<StageDto[]>([]);
    const [status, setStatus] = useState<StageDto>();
    const [name, setName] = useState<string>("");
    const [desc, setDesc] = useState<string>("");
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [deadlineDate, setDeadlineDate] = useState(new Date());
    const [isModified, setIsModified] = useState<boolean>(false);
    const [message, setMessage] = useState<string>("");
    const [currentUserId, setCurrentUserId] = useState<string | null>(null);
    const [conn, setConnection] = useState<HubConnection | null>(null);
    const [taskComments, setTaskComments] = useState<ITaskComment[]>([]);
    const [updateTaskDto, setUpdateTaskDto] = useState<UpdateTaskDto>({} as UpdateTaskDto);

    useEffect(() => {
        getTaskInfo();
        getUserId();
    }, []);

    useEffect(() => {
        if (!currentTask) return;
        if (desc !== currentTask.description || name !== currentTask.name) {
            currentTask.description = desc;
            setIsModified(true);
        }
        else setIsModified(false);
    }, [desc, name]);

    useEffect(() => {
        if (!currentTask) return;
        if (currentTask.assignee === null)
            setResponsibleUser((prev) => ({
                ...prev,
                username: "Все",
            }));
        setName(currentTask.name);
        setDesc(currentTask.description);
        // setStartDate(dayjs(currentTask.created).format("YYYY-MM-DD"));
        // setFinishDate(dayjs(currentTask.finishDate).format("YYYY-MM-DD"));
    }, [currentTask]);

    useEffect(() => {
        if (currentTask && statusList !== undefined) {
            const currentStatus = statusList.find(
                (st) => st.id === currentTask.stageId
            );
            setStatus(currentStatus);
        }
    }, [statusList]);

    const getUserId = () => {
        const userId = localStorage.getItem("userId");
        setCurrentUserId(userId);
    };

    // const handleSaveStartDate = async (date: string) => {
    //     const taskId = searchParams.get("task");
    //     if (!taskId) return;
    //     const response = await UpdateTaskStartDate(taskId, date);
    // };

    // const handleSaveFinishDate = async (date: string) => {
    //     const taskId = searchParams.get("task");
    //     if (!taskId) return;
    //     const response = await UpdateTaskFinishDate(taskId, date);
    // };

    const handleSaveChanges = async () => {
        const taskId = searchParams.get("task");
        if (!taskId) return;
        if (updateTaskDto) {
            updateTask(updateTaskDto);
            setIsModified(false);
        }
    };

    // const handleChangeResponsibleUser = async (value: string) => {
    //     const taskId = searchParams.get("task");
    //     if (!taskId) return;
    //     if (value === "Все") {
    //         const response = await UpdateTaskResponsibleUser(taskId, "");
    //         setResponsibleUser({} as IUser);
    //     } else {
    //         const newResponsibleUser = users.find((us) => us.id === value);
    //         const newUsersArr = users.filter(
    //             (user) => user.id !== newResponsibleUser!.id
    //         );
    //         setUsers([...newUsersArr, responsibleUser]);
    //         setResponsibleUser(newResponsibleUser!);
    //         const response = await UpdateTaskResponsibleUser(taskId, value);
    //     }
    // };

    // const saveName = async (taskId: string) => {
    //     const response = await UpdateTaskName(taskId, name);
    //     return response;
    // };

    // const saveDescription = async (taskId: string) => {
    //     const response = await UpdateTaskDescription(taskId, desc);
    //     return response;
    // };

    const getTaskInfo = async () => {
        const taskId = searchParams.get("task");
        if (!taskId) return;

        getTask(taskId).then(response => {
            const task = response.data;

            getStagesInfo(task.projectId);
            setTask(task);
            setUpdateTaskDto({
                taskId: task.taskId,
                name: task.name,
                description: task.description,
                index: task.index,
                priority: task.priority,
                deadlineDate: task.deadlineDate,
                stageId: task.stageId,
            });
            setTaskComments(task.comments ?? []);
            if (task.deadlineDate) {
                setDeadlineDate(new Date(task.deadlineDate));
            }

            getUsers(params.id).then(usersArr => {
                const usersWithoutResponsible = usersArr.filter((us) => us.id !== task.assignee?.id);

                setUsers(usersWithoutResponsible);
                const user = usersArr.find((us) => us.id === task.assignee?.id);
                if (user) setResponsibleUser(user!);
                else setResponsibleUser({ username: "Все" } as IUser);
            })
        });
    };

    const handleChangeStage = async (stageName: string | undefined) => {
        const taskId = searchParams.get("task");
        if (!taskId || stageName === undefined) return;
        const stage = statusList.find((st) => st.customName === stageName)!;
        updateTaskStage(taskId, stage.id)
            .then(() => {
                setStatus(stage);
            });
    };

    const getStagesInfo = async (projectId: string) => {
        getStagesByProjectId(projectId)
            .then(response => response.data)
            .then(data => {
                setStatusList(StageMapperService.mapToClient(data).sort(x => x.type));
            });
    };

    const handleDelete = async () => {
        if (!currentTask) return;
        const response = await deleteTask(currentTask.taskId);
        setFetch(true);
        onClose();
    };

    const getUsers = async (id: string) => {
        return getUsersByProjectId(id).then(x => x.data);
    };

    useEffect(() => {
        async function start() {
            let connection = new HubConnectionBuilder()
                .withUrl(`http://localhost:5000/chat`)
                .build();
            setConnection(connection);
            if (connection.state === HubConnectionState.Disconnected) {
                await connection.start();
            } else {
                console.log("Already connected");
            }
        }
        if (isActive) start();
    }, [isActive]);

    try {
        if (conn) {
            const taskId = searchParams.get("task");
            if (!taskId) return;
            conn.on(`ReceiveTaskComment${taskId}`, (comment) => {
                setTaskComments([...taskComments, comment]);
            });
        }
    } catch (exception) {
        console.log(exception);
    }

    const sendMessage = async () => {
        if (!conn) return;
        const taskId = searchParams.get("task");
        if (!taskId || !currentUserId) return;
        if (conn.state === HubConnectionState.Connected) {
            conn.invoke("SendTaskComment", currentUserId, message, taskId);
            setMessage("");
        } else {
            console.log("sendMsg: " + conn.state);
        }
    };

    return (
        <div className="bg-white dark:bg-[#111] grid grid-cols-[3fr_1fr] p-[0.4rem]">
            <div className="border-r dark:border-gray-100 h-full flex flex-col"
                style={rights && (rights & UserRights.CanModifyTask) ? {} : { pointerEvents: "none" }}
            >
                <div className="flex flex-col justify-between h-full mx-[1rem] ">
                    <div className="flex flex-col gap-3">
                        <div className="flex flex-row items-center">
                            <Label className="text-inherit text-[1rem] grow-[5]">Название:</Label>
                            <Input
                                className="text-red text-xl grow-[5] rounded-none border-0 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0"
                                onChange={(e) => setName(e.target.value)}
                                value={name}
                                placeholder={name}
                            />
                        </div>
                        <div>
                            <Label className="text-inherit text-[1rem] grow-[5]">Описание</Label>
                            <Editor desc={desc} setDesc={setDesc} styles="dark:border-black/40" />
                        </div>
                        <div className="flex flex-row justify-between gap-2">
                            <div className="flex flex-col gap-2 w-[50%]">
                                <div className="flex flex-col">
                                    <Label id="responsibleinput" className="text-black mr-[1rem] mb-[0.2rem]">Исполнитель</Label>
                                    <Select
                                        value={responsibleUser ? responsibleUser.id : "Все"}
                                        onValueChange={(value) => setUpdateTaskDto({
                                            ...updateTaskDto,
                                            assigneeId: value,
                                        })}
                                    >
                                        <SelectTrigger className="dark:bg-[#111] dark:border-black/20">
                                            <SelectValue placeholder="Выберите"></SelectValue>
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem key={"all"} value={"Все"}>
                                                <div>Все</div>
                                            </SelectItem>
                                            {responsibleUser.firstName !== undefined ? (
                                                <SelectItem
                                                    key={responsibleUser.id}
                                                    value={responsibleUser.id}
                                                >
                                                    <div className="flex flex-row items-center gap-4">
                                                        <Avatar
                                                            className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"
                                                        // src="/static/images/avatar/1.jpg"
                                                        >
                                                            {responsibleUser?.firstName?.slice(0, 1)}
                                                            {responsibleUser?.lastName?.slice(0, 1)}
                                                        </Avatar>
                                                        <div>
                                                            {responsibleUser.lastName + " " + responsibleUser.firstName}
                                                        </div>
                                                    </div>
                                                </SelectItem>
                                            ) : (
                                                <></>
                                            )}
                                            {users.map((user) => (
                                                <SelectItem key={user.id} value={user.id}>
                                                    <div className="flex flex-row items-center gap-2">
                                                        {user.username !== "Все" ? (
                                                            <Avatar
                                                                className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"
                                                            // src="/static/images/avatar/1.jpg"
                                                            >
                                                                {user?.firstName?.slice(0, 1)}
                                                                {user?.lastName?.slice(0, 1)}
                                                            </Avatar>
                                                        ) : (
                                                            <div></div>
                                                        )}
                                                        <div>
                                                            {user.username !== "Все"
                                                                ? user.lastName + " " + user.firstName
                                                                : "Все"}
                                                        </div>
                                                    </div>
                                                </SelectItem>
                                            ))}
                                        </SelectContent>
                                    </Select>
                                </div>
                                <div className="flex flex-col">
                                    <Label id="statusinput" className="text-black mr-[1rem] mb-[0.2rem]">Статус</Label>
                                    <Select value={status?.customName || ""} onValueChange={(value) => handleChangeStage(value)}>
                                        <SelectTrigger className="SelectTrigger dark:bg-[#111] dark:border-black/20">
                                            <SelectValue placeholder="Выберите стадию" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            {statusList?.map(status => (
                                                <SelectItem key={status.id} value={status.customName.toString()}>
                                                    {status.customName}
                                                </SelectItem>
                                            ))}
                                        </SelectContent>
                                    </Select>
                                </div>
                                <div className="flex flex-col">
                                    <Label className="mb-[0.2rem]">Дедлайн</Label>
                                    <Popover>
                                        <PopoverTrigger asChild className="dark:bg-[#111] dark:border-black/20  dark:hover:bg-black/20">
                                            <Button variant={"outline"}
                                                className={cn(
                                                    "w-[240px] justify-start text-left font-normal w-full",
                                                    !deadlineDate && "text-muted-foreground"
                                                )}
                                            >
                                                <CalendarIcon className="mr-2 h-4 w-4" />
                                                {deadlineDate ? deadlineDate.toDateString() : <span>Выберите дедлайн</span>}
                                            </Button>
                                        </PopoverTrigger>
                                        <PopoverContent className="w-auto p-0" align="start">
                                            <Calendar
                                                mode="single"
                                                selected={deadlineDate}
                                                onSelect={(value) => {
                                                    if (value) {
                                                        setDeadlineDate(value);
                                                        setUpdateTaskDto({
                                                            ...updateTaskDto,
                                                            deadlineDate: value,
                                                        })
                                                    }
                                                    // handleSaveFinishDate(dayjs(value).format("YYYY-MM-DD"));
                                                }}
                                            />
                                        </PopoverContent>
                                    </Popover>
                                </div>
                            </div>
                            {/* <div className="flex flex-col gap-2 w-[50%]">
                                <div className="flex flex-row items-center gap-2">
                                    <Label>Создана:</Label>
                                    <span>{dayjs(startDate).format("YYYY-MM-DD")}</span>
                                </div>
                                <div className="flex flex-row items-center gap-2">
                                    <Label>Последнее изменение:</Label>
                                    <span>{dayjs(startDate).format("YYYY-MM-DD")}</span>
                                </div>
                            </div> */}
                        </div>
                    </div>
                    <div className="flex flex-row justify-between">
                        <Button
                            // className={
                            //     isModified
                            //         ? "border border-green-500 bg-white text-green-500 hover:bg-green-500 hover:text-white ease-in-out duration-300 dark:bg-green-500 dark:text-white dark:hover:bg-green-600"
                            //         : "invisible"
                            // }
                            onClick={() => handleSaveChanges()}
                        >
                            Сохранить
                        </Button>
                        <Button
                            variant={"destructive"}
                            className={rights && (rights & UserRights.CanDeleteTask) ? "" : "invisible"}
                            onClick={() => setIsModalOpen(true)}
                        >
                            Удалить
                        </Button>
                    </div>
                </div>
            </div>
            <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
                <DialogContent className="sm:max-w-[425px]">
                    <div className="flex flex-col justify-center bg-white w-full gap-2 rounded-[8px] p-[10px]">
                        <Label className="text-black self-center">
                            Вы действительно хотите удалить задачу?
                        </Label>
                        <div className="text-black self-center">
                            <Button
                                className="text-white bg-green-600 hover:bg-green-700 mr-[0.4rem]"
                                onClick={() => handleDelete()}
                            >
                                Да
                            </Button>
                            <Button
                                className="text-white bg-red-600 hover:bg-red-900"
                                onClick={() => setIsModalOpen(false)}
                            >
                                Нет
                            </Button>
                        </div>
                    </div>
                </DialogContent>
            </Dialog>
            <div className="min-w-[20rem]">
                <div className="h-full flex flex-col justify-between">
                    <div className="flex flex-row p-[0.6rem] justify-between items-center">
                        <Label className="flex-1 text-center">{`Комментарии`}</Label>
                        <X className="text-[1.6rem] p-[0.1rem] self-center hover:rounded-[12px] ease-in-out duration-200"
                            onClick={() => {
                                onClose();
                                setFetch(true);
                            }}
                        />
                    </div>
                    <ScrollArea className="max-h-[58vh] min-h-[48vh] overflow-auto flex flex-col px-[1rem] grow">
                        {taskComments?.map((message, index) =>
                            currentUserId && message.userId === currentUserId ? (
                                <div key={index} className="flex flex-col">
                                    <div className="self-end">
                                        <Label className="text-center text-[0.7rem] text-[#87888C]">
                                            {message
                                                ? format(message.time, "DD.MM.YY HH:mm")
                                                : ""}
                                        </Label>
                                    </div>
                                    <div className="flex justify-end my-[0.15rem]">
                                        <div className="bg-[#3288F0] px-[0.8rem] py-[0.2rem] rounded-[0.6rem]">
                                            <div className="flex justify-between items-baseline">
                                                <Label className="text-white dark:text-white">
                                                    {message.content}
                                                </Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            ) : (
                                <div key={index}>
                                    <div>
                                        <Label className="text-center text-[0.7rem] text-[#87888C]">
                                            {message
                                                ? format(message.time, "DD.MM.YY HH:mm")
                                                : ""}
                                        </Label>
                                    </div>
                                    <div className="flex justify-start my-[0.15rem]">
                                        <div className="bg-[#ECF0F3] px-[0.8rem] py-[0.2rem] rounded-[0.6rem]">
                                            <Label className="text-[#87888C] text-[0.7rem]">
                                                {message.username}
                                            </Label>
                                            <div className="flex justify-between items-baseline">
                                                <Label>{message.content}</Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            )
                        )}
                    </ScrollArea>
                    <div className="px-[0.8rem] pt-[0.4rem] rounded-[0.6rem] flex justify-between items-center w-full gap-2 mt-[0.5rem]">
                        <Input
                            className="flex-1 dark:text-white dark:bg-[#111] dark:border-black/20"
                            placeholder="Написать сообщение..."
                            value={message}
                            onChange={(e) => setMessage(e.target.value)}
                        />
                        <Send
                            className="text-[0.9rem] text-[rgba(102,153,255,0.6)]"
                            onClick={() => sendMessage()}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default EditTaskDialog;
