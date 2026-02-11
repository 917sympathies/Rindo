"use client";
import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import Link from "next/link";
import { Label } from "../ui/label";
import { getTasksByUserId } from "@/requests";
import { format } from "date-fns";
import {ProjectTasks} from "@/types/project.types";

export default function UserTasks() {
    const [projects, setProjects] = useState<ProjectTasks[]>([]);

    useEffect(() => {
        fetchTasks();
    }, []);

    const fetchTasks = async () => {
        const userId = localStorage.getItem("userId");
        const response = await getTasksByUserId(userId!);
        const data = response.data;
        setProjects(data);
    };

    return (
        <>
            <div className="flex flex-col flex-wrap gap-2">
                {projects &&
                    projects.map((project) => (
                        <div key={project.projectName} className={project.tasks.length > 0 ? "bg-blue-400 rounded-lg p-4 w-fit" : ""}>
                            {project.tasks.length > 0 ?
                                <div className="flex flex-row gap-4 items-center">
                                    <span className="text-[1.2rem] font-semibold m-2 text-white">{project.projectName}</span>
                                    <Link className="text-gray-500 font-medium text-[0.8rem] bg-gray-50 hover:bg-gray-50 ease-in-out duration-300 pl-2 pr-2 pb-1 pt-1 rounded-full" href={`/project/${project.projectId}/board`}>
                                        Перейти к проекту
                                    </Link>
                                </div> : <></>}
                            <div className="flex flex-row flex-nowrap gap-2">
                                {project.tasks &&
                                    project.tasks.map((task) => (
                                        <Card className="dark:bg-[#111] dark:border-black/20 flex flex-col justify-between" key={task.taskId}>
                                            <CardHeader>
                                                <CardTitle className="flex flex-row gap-2 justify-between">
                                                    <div className="flex flex-row items-center gap-2">{task.name}</div>
                                                </CardTitle>
                                                <CardDescription className="flex flex-col dark:text-gray-400">
                                                    <textarea
                                                        style={{
                                                            border: "0",
                                                            resize: "none",
                                                            fontFamily: "inherit",
                                                            backgroundColor: "inherit",
                                                            overflow: "hidden",
                                                        }}
                                                        disabled
                                                        cols={40}
                                                        rows={(task.description.length / 40 + 1) > 2 ? 2 : (task.description.length / 40 + 1)}
                                                        value={task.description}
                                                    ></textarea>
                                                </CardDescription>
                                            </CardHeader>
                                            <CardContent>
                                                <div className="flex flex-col">
                                                    <div className="mb-1 flex flex-row items-center gap-1">
                                                        <Label className="font-medium text-gray-500">Нужно выполнить до:</Label>
                                                        {
                                                            task.deadlineDate ?
                                                                <Label className="p-1 pl-2 pr-2 bg-orange-700 text-white rounded-full">{format(task.deadlineDate, "dd.MM.yyyy")}</Label>
                                                                :
                                                                <Label className="p-1 pl-2 pr-2 bg-orange-700 text-white rounded-full">Дедлайна нет</Label>
                                                        }
                                                    </div>
                                                </div>
                                            </CardContent>
                                        </Card>
                                    ))}
                            </div>
                        </div>
                    ))}
            </div>
        </>
    );
}
