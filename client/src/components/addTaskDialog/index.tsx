import {useParams} from "next/navigation";
import {addTask as addTaskRequest, getStagesByProjectId, getUsersByProjectId} from "@/requests";
import React, {Dispatch, SetStateAction, useEffect, useState} from "react";
import {AddTaskDto, StageShortInfo, StageType, TaskDto, TaskPriority, UserDto} from "@/types";
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {Input} from "@/components/ui/input";
import {Label} from "@/components/ui/label";
import {Button} from "@/components/ui/button";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select";
import {Avatar} from "@/components/ui/avatar";
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import {cn} from "@/lib/utils";
import {Calendar as CalendarIcon, X} from "lucide-react";
import {Calendar} from "@/components/ui/calendar";
import {format} from "date-fns";
import Editor from "@/components/editor";
import {EnumUtils} from "@/utils/enum.utils";
import {DialogClose, DialogFooter, DialogHeader} from "@/components/ui/dialog";
import {StageMapperService} from "@/utils/stage-mapper.service";


interface Props {
    onClose: () => void;
}

const AddTaskDialog = ({ onClose }: Props) => {
    const { id } = useParams<{ id: string }>();
    const [users, setUsers] = useState<UserDto[]>([]);
    const [stages, setStages] = useState<StageShortInfo[]>([]);
    const [conn, setConnection] = useState<HubConnection | null>(null);
    const [taskToAdd, setTaskToAdd] = useState<AddTaskDto>({} as AddTaskDto);
    const [taskPriorities, _] = useState<TaskPriority[]>(EnumUtils.getEnumNumbers(TaskPriority));

    const addTask = () => {

        const taskDto: AddTaskDto = {
            id: crypto.randomUUID(),
            name: taskToAdd.name,
            description: taskToAdd.description,
            projectId: id,
            stageId: taskToAdd.stageId,
            assigneeId: taskToAdd.assigneeId,
            deadlineDate: taskToAdd.deadlineDate,
            priority: TaskPriority.Medium,
            reporterId: undefined,
        };

        addTaskRequest(taskDto)
            .then(() => {
                sendSignal();
                // setFetch(true);
                onClose();
            });
    }

    const sendSignal = async () => {
        if (!conn) return;
        if (conn.state === HubConnectionState.Connected) {
            conn.invoke("SendTaskAdd", id);
        } else {
            console.log("sendMsg: " + conn.state);
        }
    };

    const getUsers = async (projectId: string) => {
        const response = await getUsersByProjectId(projectId);
        const data = response.data;
        setUsers(data);
    };

    const getStages = (projectId: string) => {
        getStagesByProjectId(projectId)
            .then(res => res.data)
            .then(stages => {
                setTaskToAdd({
                    ...taskToAdd,
                    stageId: stages.find(s => s.type === StageType.ToDo)!.id
                })
            })
    }

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

    useEffect(() => {
        start();
        getUsers(id);
        getStages(id);
    }, [])


    return (
        <div className="flex flex-col gap-2">
            <DialogHeader className="flex flex-row items-center justify-between">
                <h1>Добавить задачу</h1>
                <DialogClose asChild>
                    <Button variant={"icon"} size={"icon"} onClick={onClose}>
                        <X size={16} />
                    </Button>
                </DialogClose>
            </DialogHeader>
            <div className="input-container">
                <Label>Название</Label>
                <Input
                    placeholder="Название"
                    value={taskToAdd.name}
                    onChange={(e) =>
                        setTaskToAdd({
                            ...taskToAdd,
                            name: e.target.value,
                        })
                    }
                ></Input>
            </div>
            <div className="input-container">
                <Label>Ответственный</Label>
                <Select value={taskToAdd.assigneeId} onValueChange={(value) => setTaskToAdd({
                    ...taskToAdd,
                    assigneeId: value,
                })}>
                    <SelectTrigger className="SelectTrigger dark:bg-[#111] dark:border-black/20">
                        <SelectValue placeholder="Все" />
                    </SelectTrigger>
                    <SelectContent className="dark:bg-[#111]">
                        <SelectItem className="dark:hover:bg-black pl-[2.5rem]" value={"все"}>
                            Все
                        </SelectItem>
                        {users &&
                            users.map((user) => (
                                <SelectItem key={user.username} value={user.id}>
                                    <div className="flex flex-row items-center gap-2">
                                        <Avatar className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"
                                            // src="/static/images/avatar/1.jpg"
                                        >
                                            {user?.firstName?.slice(0, 1)}
                                            {user?.lastName?.slice(0, 1)}
                                        </Avatar>
                                        <div>
                                            { `${user.firstName} ${user.lastName} (${user.username})`}
                                        </div>
                                    </div>
                                </SelectItem>
                            ))}
                    </SelectContent>
                </Select>
            </div>
            <div className="input-container">
                <Label>Приоритет</Label>
                <Select value={String(taskToAdd.priority)} onValueChange={(value) => setTaskToAdd({
                    ...taskToAdd,
                    priority: +value,
                })}>
                    <SelectTrigger className="dark:bg-[#111] dark:border-black/20">
                        <SelectValue>Выберите приоритет задачи</SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                        {
                            taskPriorities?.map((priority) => (
                                <SelectItem key={priority} value={String(priority)}>
                                    <span>{String(priority)}</span>
                                </SelectItem>
                            ))
                        }
                    </SelectContent>
                </Select>
            </div>
            <div className="input-container">
                <Label>Конец</Label>
                <Popover>
                    <PopoverTrigger asChild>
                        <Button
                            variant={"outline"}
                            className={cn(
                                "w-[240px] justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20",
                                !taskToAdd.deadlineDate && "text-muted-foreground"
                            )}
                        >
                            <CalendarIcon className="mr-2 h-4 w-4" />
                            {taskToAdd.deadlineDate ? (
                                format(taskToAdd.deadlineDate, "dd.MM.yyyy")
                            ) : (
                                <span>Выберите дату</span>
                            )}
                        </Button>
                    </PopoverTrigger>
                    <PopoverContent className="w-auto p-0" align="start">
                        <Calendar
                            mode="single"
                            selected={taskToAdd.deadlineDate}
                            onSelect={(value) => {
                                if (value) setTaskToAdd({
                                    ...taskToAdd,
                                    deadlineDate: value,
                                })
                            }}
                        />
                    </PopoverContent>
                </Popover>
            </div>
            <div className="input-container">
                <Label>Описание задачи</Label>
                <div className="relative overflow-x-hidden overwlow-y-auto max-w-full">
                    <Editor
                        desc={taskToAdd.description}
                        setDesc={() => setTaskToAdd({
                            ...taskToAdd,
                            description: taskToAdd.description,
                        })}
                        styles="dark:border-black/40"
                    />
                </div>
            </div>
            <DialogFooter>
                <Button onClick={() => addTask()}>Добавить</Button>
            </DialogFooter>
        </div>
    );
}

export default AddTaskDialog;