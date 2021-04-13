import shutil

import cv2 as cv
import numpy as np
import time
import os
import imutils


# def definition(image):
#     gray = cv.cvtColor(image, cv.COLOR_BGR2GRAY)
#     result = cv.Laplacian(gray, cv.CV_64F).var()
#     print('ai_hellohello.jpg blur:', result)



def del_file(filepath):
    del_list = os.listdir(filepath)
    for f in del_list:
        file_path = os.path.join(filepath, f)
        if os.path.isfile(file_path):
            os.remove(file_path)
        elif os.path.isdir(file_path):
            shutil.rmtree(file_path)
    os.rmdir(filepath)


def small(filepath,small):
    img_dir = filepath
    names = os.listdir(img_dir)
    images = []
    for name in names:
        img_path = os.path.join(img_dir, name)
        image = cv.imread(img_path)
        images.append(image)
    for i in range(0,len(images)):
        image = images[i]
        height, width = image.shape[:2]
        # cv.imshow('image', image)
        image = cv.resize(image, (int(width / small), int(height / small)), interpolation=cv.INTER_CUBIC)
        cv.imwrite(filepath+"\\"+names[i],image)


def take_photo(filepath,num,timerfunc,small):
    cap = cv.VideoCapture(1)
    i = 1
    while (i < num+1):  # 读取10张图片
        ret, frame = cap.read()
        height, width = frame.shape[:2]
        cv.imshow('frame', frame)
        frame = cv.resize(frame, (int(width / small), int(height / small)), interpolation=cv.INTER_CUBIC)
        frame = rotate(frame)
        cv.imwrite(filepath + "\\" + os.path.split(filepath)[1] + "_" + str(i) + ".jpg", frame)
        print(filepath + "\\" + os.path.split(filepath)[1] + "_" + str(i) + ".jpg")
        time.sleep(timerfunc)
        i = i + 1
        if cv.waitKey(1) & 0xFF == ord('q'):
            break
    cap.release()
    cv.destroyAllWindows()


def rotate(img):      #旋转照片
    height, width = img.shape[:2]
    matRotate = cv.getRotationMatrix2D((height * 0.5, width * 0.5), -90, 1)
    dst = cv.warpAffine(img, matRotate, (width, height * 2))
    rows, cols = dst.shape[:2]
    for col in range(0, cols):
        if dst[:, col].any():
            left = col
            break
    for col in range(cols - 1, 0, -1):
        if dst[:, col].any():
            right = col
            break
    for row in range(0, rows):
        if dst[row, :].any():
            up = row
            break
    for row in range(rows - 1, 0, -1):
        if dst[row, :].any():
            down = row
            break
    res_widths = abs(right - left)
    res_heights = abs(down - up)
    res = np.zeros([res_heights, res_widths, 3], np.uint8)
    for res_width in range(res_widths):
        for res_height in range(res_heights):
            res[res_height, res_width] = dst[up + res_height, left + res_width]
    return res

def rotate2(img):      #旋转照片
    height, width = img.shape[:2]
    matRotate = cv.getRotationMatrix2D((height * 0.5, width * 0.5), 90, 1)
    dst = cv.warpAffine(img, matRotate, (width, height * 2))
    rows, cols = dst.shape[:2]
    for col in range(0, cols):
        if dst[:, col].any():
            left = col
            break
    for col in range(cols - 1, 0, -1):
        if dst[:, col].any():
            right = col
            break
    for row in range(0, rows):
        if dst[row, :].any():
            up = row
            break
    for row in range(rows - 1, 0, -1):
        if dst[row, :].any():
            down = row
            break
    res_widths = abs(right - left)
    res_heights = abs(down - up)
    res = np.zeros([res_heights, res_widths, 3], np.uint8)
    for res_width in range(res_widths):
        for res_height in range(res_heights):
            res[res_height, res_width] = dst[up + res_height, left + res_width]
    return res

def read_video(filepath,savepath,photonum,small):
    cap = cv.VideoCapture(filepath)
    num=0
    frames_num = cap.get(7)
    print(frames_num)
    timerF = int(frames_num/photonum)
    while (cap.isOpened()):
        ret, frame = cap.read()
        if(ret == True):
            if(num%timerF==0):
                height, width = frame.shape[:2]
                frame = cv.resize(frame, (int(width / small), int(height / small)), interpolation=cv.INTER_CUBIC)
                frame = rotate2(frame)
                cv.imshow('frame',frame)
                path = savepath +"/"+str(int(num/timerF)) + '.jpg'
                cv.imwrite(path, frame)
                if cv.waitKey(1) & 0xFF == ord('q'):
                    break
            num = num + 1
        else:
           break
    cap.release()
    cv.destroyAllWindows()

def operation(path):
    img_dir = path
    names = os.listdir(img_dir)
    images = []
    for name in names:
        img_path = os.path.join(img_dir, name)
        image = cv.imread(img_path)
        images.append(image)
    # stitcher = cv.createStitcher() if imutils.is_cv3() else cv.Stitcher_create()
    # compare = Compare()
    # compare.stitchdifferent(img_dir)
    stitcher = cv.Stitcher_create()
    status, stitched = stitcher.stitch(images)
    if status == 0:
        cv.imwrite(img_dir + '/stitch.jpg', stitched)
        print("合成成功")
        stitched = cv.copyMakeBorder(stitched, 10, 10, 10, 10,
                                     cv.BORDER_CONSTANT, (0, 0, 0))
        gray = cv.cvtColor(stitched, cv.COLOR_BGR2GRAY)
        ret, thresh = cv.threshold(gray, 0, 255, cv.THRESH_BINARY)
        cnts, hierarchy = cv.findContours(thresh.copy(), cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
        cnt = max(cnts, key=cv.contourArea)  # 获取最大轮廓
        mask = np.zeros(thresh.shape, dtype="uint8")
        x, y, w, h = cv.boundingRect(cnt)
        # 绘制最大外接矩形框（内部填充）
        cv.rectangle(mask, (x, y), (x + w, y + h), 255, -1)
        minRect = mask.copy()
        sub = mask.copy()
        # 开始while循环，直到sub中不再有前景像素
        while cv.countNonZero(sub) > 0:
            minRect = cv.erode(minRect, None)
            sub = cv.subtract(minRect, thresh)
        cnts, hierarchy = cv.findContours(minRect.copy(), cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
        cnt = max(cnts, key=cv.contourArea)
        x, y, w, h = cv.boundingRect(cnt)
        # 使用边界框坐标提取最终的全景图
        stitched = stitched[y:y + h, x:x + w]
        cv.imwrite(img_dir + '/final.jpg', stitched)
    else:
        print(status)
        print("合成失败")
        if(status==1):
            print("输入图像中没有检测到足够的关键点来构建全景图。 您将需要更多输入图像")
        elif(status==2):
            print("RANSAC单应性估计失败时发生此错误。 同样，您可能需要更多输入图像，或者所提供的图像没有足够的区别特征来准确匹配关键点")
        elif(status==3):
            print("通常与未能正确估计输入图像中的相机特征有关。")

def dealimage(path,result):
    stitched = cv.imread(path)
    stitched = cv.copyMakeBorder(stitched, 10, 10, 10, 10,
                           cv.BORDER_CONSTANT, (0, 0, 0))

    gray = cv.cvtColor(stitched, cv.COLOR_BGR2GRAY)
    # cv.imshow("1", gray)
    # cv.imshow("2", thresh)
    # cv.waitKey(-1)
    ret, thresh = cv.threshold(gray, 0, 255, cv.THRESH_BINARY)
    cnts, hierarchy = cv.findContours(thresh.copy(), cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
    cnt = max(cnts, key=cv.contourArea)  # 获取最大轮廓
    mask = np.zeros(thresh.shape, dtype="uint8")
    x, y, w, h = cv.boundingRect(cnt)
    # 绘制最大外接矩形框（内部填充）
    cv.rectangle(mask, (x, y), (x + w, y + h), 255, -1)
    minRect = mask.copy()
    sub = mask.copy()
    # 开始while循环，直到sub中不再有前景像素
    while cv.countNonZero(sub) > 0:
        minRect = cv.erode(minRect, None)
        sub = cv.subtract(minRect, thresh)
    cnts, hierarchy = cv.findContours(minRect.copy(), cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
    cnt = max(cnts, key=cv.contourArea)
    x, y, w, h = cv.boundingRect(cnt)
    # 使用边界框坐标提取最终的全景图
    cv.imshow("1",stitched)
    stitched = stitched[y:y + h, x:x + w]
    cv.imshow("result",stitched)
    print(stitched.shape)
    cv.waitKey(-1)
    cv.imwrite(result, stitched)


def save(path,images):
    i = 1
    for image in images:
        cv.imwrite(path + "\\" + str(i) + ".jpg",image)


#通过摄像头读取照片 path图片保存路径 num图片数量 timerfunc每隔多少秒 small缩小照片的倍数
def operation1(path,num,timerfunc,small):
    img_dir = path
    if(os.path.exists(img_dir)):
        del_file(img_dir)
    os.makedirs(img_dir)
    take_photo(img_dir,num,timerfunc,small)
    operation(img_dir)

#videopath视频路径 savepath 图片保存路径 num表示照片数量 small表示缩小照片的倍数
def operation2(videopath,savepath,num,small):
    if (os.path.exists(savepath)):
        del_file(savepath)
    os.makedirs(savepath)
    read_video(videopath, savepath, num,small)
    operation(savepath)



t0 = time.time()
# operation1("image/23",15,3,5)
# operation2("video/4.5.1.mp4","image/35",30,5)
# small("image/44",20)
operation("D:\\QQdownload\\14")
t1 = time.time()
print(str(t1 - t0))
# dealimage("image/result.jpg","image/end.jpg")

