"use client";
import { ITask } from "@/types";
import { useState, useEffect } from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardFooter,
  CardTitle,
} from "@/components/ui/card";
import { Progress } from "../ui/progress";
import { Button } from "../ui/button";
import Link from "next/link";
import { Label } from "../ui/label";
import dayjs from "dayjs";

interface IProjectTasks {
  name: string;
  id: string;
  tasks: ITask[];
}

export default function UserTasks() {
  const [projects, setProjects] = useState<IProjectTasks[]>([]);

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    const userId = localStorage.getItem("userId");
    const response = await fetch(
      `http://localhost:5000/api/project/${userId}/usertasks`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const data = await response.json();
    console.log(data);
    setProjects(data);
  };

  useEffect(() => {
    console.log(projects);
  }, [projects]);

  return (
    <>
      <div className="flex flex-col flex-wrap gap-2">
        {projects &&
          projects.map((project) => (
            <div key={project.name} className={project.tasks.length > 0 ? "bg-blue-400 rounded-lg p-4 w-fit" : ""}>
              {project.tasks.length > 0 ?
              <div className="flex flex-row gap-4 items-center">
                <span className="text-[1.2rem] font-semibold m-2 text-white">{project.name}</span>
                <Link className="text-gray-500 font-medium text-[0.8rem] bg-gray-50 hover:bg-gray-50 ease-in-out duration-300 pl-2 pr-2 pb-1 pt-1 rounded-full" href={`/project/${project.id}/board`}>
                  Перейти к проекту
                </Link>
              </div> : <></> }
              <div className="flex flex-row flex-nowrap gap-2">
                {project.tasks &&
                  project.tasks.map((task) => (
                    <Card
                      className="dark:bg-[#111] dark:border-black/20 flex flex-col justify-between"
                      key={task.id}
                    >
                      <CardHeader>
                        <CardTitle className="flex flex-row gap-2 justify-between">
                          <div className="flex flex-row items-center gap-2">
                            {task.name}
                          </div>
                        </CardTitle>
                        <CardDescription className="flex flex-col dark:text-gray-400">
                          <div>
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
                              rows={ (task.description.length / 40 + 1) > 2 ? 2 : (task.description.length / 40 + 1)}
                              value={task.description}
                            ></textarea>
                          </div>
                          <div></div>
                        </CardDescription>
                      </CardHeader>
                      <CardContent>
                        <div className="flex flex-col">
                          <div className="mb-1 flex flex-row items-center gap-1">
                            <Label className="font-medium text-gray-500">Нужно выполнить до:</Label>
                            <Label className="p-1 pl-2 pr-2 bg-orange-700 text-white rounded-full">{dayjs(task.finishDate).format("DD MM YYYY")}</Label>
                          </div>
                          <div className="flex flex-col items-center">
                            <Label className="w-full mb-1 ml-1 text-[0.7rem] text-gray-500">Прогресс</Label>
                            <Progress value={task.progress} />
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
