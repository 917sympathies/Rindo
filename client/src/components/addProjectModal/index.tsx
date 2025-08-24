"use client";
import { useState, useEffect, useRef, Dispatch, SetStateAction } from "react";
import { Label } from "../ui/label";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { cn } from "@/lib/utils";
import { Calendar as CalendarIcon, CirclePlus } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import React from "react";
import { useCookies } from "react-cookie";
import { useRouter } from "next/navigation";
import { jwtDecode } from "jwt-decode";
import { CookieInfo, IProjectDto } from "@/types";
import { X } from "lucide-react";
import dayjs from "dayjs";
import Editor from "../editor";
import { CreateProject } from "@/requests";

interface IAddProjectModalProps {
  setFetch: Dispatch<SetStateAction<boolean>>;
  onClose: () => void;
}

const AddProjectModal = ({ setFetch, onClose }: IAddProjectModalProps) => {
  const router = useRouter();
  const [cookies, setCookie, removeCookie] = useCookies(["_rindo"]);
  const [project, setProject] = useState<IProjectDto>({} as IProjectDto);
  const [tagName, setTagName] = useState<string>("");
  const [startDate, setStart] = useState(dayjs().format("YYYY-MM-DD"));
  const [finishDate, setFinish] = useState(dayjs().format("YYYY-MM-DD"));
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [desc, setDesc] = useState<string>("");

  const handleCreateProject = async () => {
    if (new Date(startDate) > new Date(finishDate)) {
      setErrorMessage("Вы выбрали некорректные даты!");
      return;
    }
    const userId = localStorage.getItem("userId");
    project.ownerId = userId!;
    const response = await CreateProject(project);
    const data = await response.json();
    if (data.errors === undefined) {
      setFetch(true);
      onClose();
    } else {
      if (data.errors.Name !== undefined) setErrorMessage(data.errors.Name);
    }
  };

  useEffect(() => {
    project.tags = [];
  }, []);

  useEffect(() => {
    setProject((prevState) => ({
      ...prevState,
      finishDate: finishDate,
    }));
  }, [finishDate]);

  useEffect(() => {
    setProject((prevState) => ({
      ...prevState,
      startDate: startDate,
    }));
  }, [startDate]);

  useEffect(() => {
    setProject((prevState) => ({
      ...prevState,
      description: desc,
    }));
  }, [desc]);

  return (
    <>
      <div className="max-w-[100vw] grid grid-rows-[1fr_15fr_1fr] h-full px-[3rem] py-[1rem]">
        <div className="bg-inherit flex justify-between items-center">
          <Label className="whitespace-nowrap overflow-hidden text-ellipsis text-[1.8rem] font-medium">
            Проект
          </Label>
          <X
            size={28}
            className="hover:bg-gray-50 transition ease-in-out duration-300 rounded-full p-[0.4vh] cursor-pointer"
            onClick={onClose}
          />
        </div>
        <div>
          <div className="mb-[.5rem] flex flex-col gap-2">
            <Label className="text-[1.4rem] font-medium whitespace-nowrap overflow-hidden text-ellipsis">
              Название проекта
            </Label>
            <Input
              onChange={(event) => {
                setErrorMessage("");
                setProject((prevState) => ({
                  ...prevState,
                  name: event.target.value,
                }));
              }}
              placeholder="Без названия"
              className="w-full dark:border-black/30 text-[1.1rem] font-normal"
            />
          </div>
          <div className="flex flex-col gap-1 pt-2">
            <Label className="text-[0.9rem] text-gray-700">
              Добавьте теги своему проекту
            </Label>
            <div className="flex flex-row flex-wrap content-center items-center gap-1">
              <Input
                placeholder="название тега"
                value={tagName}
                onChange={(e) => setTagName(e.target.value)}
                className="w-[30%] h-[1.8rem] rounded-full"
              ></Input>
              <CirclePlus
                className="hover:cursor-pointer"
                size={20}
                color={"gray"}
                onClick={() => {
                  setProject((prev) => ({
                    ...prev,
                    tags: [...prev.tags, { name: tagName.toLowerCase() }],
                  }));
                  setTagName("");
                }}
              />
              {project.tags?.map((tag) => (
                <div className="bg-gray-100 pr-3 pl-3 rounded-full">
                  <span className="text-[0.9rem]">{tag.name}</span>
                </div>
              ))}
            </div>
          </div>
          <div className="flex flex-col max-w-[calc(50vw - 6rem)]">
            <Label className="text-black text-[1.4rem] font-medium whitespace-nowrap overflow-hidden text-ellipsis mt-[1.5rem] mb-[0.5rem]">
              Описание проекта
            </Label>
            <div
              className="relative overflow-x-hidden overwlow-y-auto max-w-full dark:border-black/40 focus:border-0 focus-visible:outline-none focus-visible:ring-0">
              <Editor
                desc={desc}
                setDesc={setDesc}
                styles="dark:border-black/40"
              />
            </div>
          </div>
          <div
            style={{
              marginTop: "1rem",
              display: "flex",
              flexDirection: "column",
              gap: 8,
            }}
          >
            <div style={{ display: "flex", flexDirection: "column" }}>
              <Label className="mb-[0.2rem] text-[1rem]">Начало проекта</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-[240px] justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20",
                      !startDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {startDate ? (
                      dayjs(startDate).format("YYYY-MM-DD")
                    ) : (
                      <span>Pick a date</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    mode="single"
                    selected={new Date(startDate)}
                    onSelect={(value) =>
                      setStart(dayjs(value).format("YYYY-MM-DD"))
                    }
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </div>
            <div style={{ display: "flex", flexDirection: "column" }}>
              <Label className="mb-[0.2rem] text-[1rem]">Конец проекта</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-[240px] justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20",
                      !finishDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {finishDate ? (
                      dayjs(finishDate).format("YYYY-MM-DD")
                    ) : (
                      <span>Pick a date</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    mode="single"
                    selected={new Date(finishDate)}
                    onSelect={(value) =>
                      setFinish(dayjs(value).format("YYYY-MM-DD"))
                    }
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </div>
          </div>
          <div>
            {errorMessage === "" ? (
              <div></div>
            ) : (
              <p style={{ margin: "2rem 0", color: "red" }}>{errorMessage}</p>
            )}
          </div>
        </div>
        <div>
          <Button
            className="bg-sky-600 border border-sky-600 text-[1rem] text-white hover:bg-sky-700 ease-in-out"
            onClick={() => handleCreateProject()}
          >
            Создать проект
          </Button>
        </div>
      </div>
    </>
  );
};

export default AddProjectModal;
