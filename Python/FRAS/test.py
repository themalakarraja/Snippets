import cv2
import numpy as np
import face_recognition
import os
from datetime import datetime
import pickle
import xlsxwriter
import getpass

classNameFileName = 'class_names.pkl'
encodingFileName = 'face_encoding.pkl'

def findEncodings():
    path = 'Training_images'
    images = []
    classNames = []
    myList = os.listdir(path)
    print('Start finding class names..')
    for cl in myList:
        curImg = cv2.imread(f'{path}/{cl}')
        images.append(curImg)
        classNames.append(os.path.splitext(cl)[0])
    with open(classNameFileName, 'wb') as f:
        pickle.dump(classNames, f)
    print('Class names finding completed..')
    print(classNames)

    print('Start Encoding..')
    encodeList = []
    for img in images:
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        encode = face_recognition.face_encodings(img)[0]
        encodeList.append(encode)
    with open(encodingFileName, 'wb') as f:
        pickle.dump(encodeList, f)
    print('Encoding completed..')


# def markAttendance(name):
#     with open('Attendance.csv', 'r+') as f:
#         myDataList = f.readlines()

#         nameList = []
#         for line in myDataList:
#             entry = line.split(',')
#             nameList.append(entry[0])
#             if name not in nameList:
#                 now = datetime.now()
#                 dtString = now.strftime('%H:%M:%S')
#                 f.writelines(f'\n{name},{dtString}')

def markAttendance(name):
    attendanceFileName ="attendance.txt"; 
    if os.path.exists(attendanceFileName):
        with open(attendanceFileName,"a") as file:
            file.write(name + ", " + str(datetime.now()) + "\n")
    else:    
        with open(attendanceFileName,"w") as file:
            file.write(name + ", " + str(datetime.now()) + "\n")


def faceRecognize():

    with open(classNameFileName, 'rb') as f:
        classNames = pickle.load(f)

    with open(encodingFileName, 'rb') as f:
        encodeListKnown = pickle.load(f)
    # encodeListKnown = findEncodings(images)

    cam = cv2.VideoCapture(0)
    # cam = cv2.VideoCapture(getpass.rtsp)
    # cam = cv2.VideoCapture('r.avi')

    while cam.isOpened():
        ret, frame = cam.read()
        if ret:
            imgS = cv2.resize(frame, (0, 0), None, 0.25, 0.25)
            imgS = cv2.cvtColor(imgS, cv2.COLOR_BGR2RGB)

            facesCurFrame = face_recognition.face_locations(imgS, number_of_times_to_upsample=2)
            encodesCurFrame = face_recognition.face_encodings(imgS, facesCurFrame)

            for encodeFace, faceLoc in zip(encodesCurFrame, facesCurFrame):
                matches = face_recognition.compare_faces(encodeListKnown, encodeFace)
                faceDis = face_recognition.face_distance(encodeListKnown, encodeFace)

                matchIndex = np.argmin(faceDis)

                if matches[matchIndex]:
                    name = classNames[matchIndex].upper()

                    y1, x2, y2, x1 = faceLoc
                    y1, x2, y2, x1 = y1 * 4, x2 * 4, y2 * 4, x1 * 4
                    cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 255, 0), 2)
                    cv2.rectangle(frame, (x1, y2 - 35), (x2, y2), (0, 255, 0), cv2.FILLED)
                    cv2.putText(frame, name, (x1 + 6, y2 - 6), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 255, 255), 2)
                    markAttendance(name)
                    print(name, datetime.now())

                if not matches[matchIndex]:
                    name = "Unknown"

                    y1, x2, y2, x1 = faceLoc
                    y1, x2, y2, x1 = y1 * 4, x2 * 4, y2 * 4, x1 * 4
                    cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 0, 255), 2)
                    cv2.rectangle(frame, (x1, y2 - 35), (x2, y2), (0, 0, 255), cv2.FILLED)
                    cv2.putText(frame, name, (x1 + 6, y2 - 6), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 255, 255), 2)
                    markAttendance("Unknown")
                    print("Unknown", datetime.now())

            cv2.imshow('Webcam', frame)
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

    cam.release()
    cv2.destroyAllWindows()

print("1. Face Recognize")
print("2. Face Encoding")

ip = input("Enter input: ")
if ip == "1":
    faceRecognize()
elif ip == "2":
    findEncodings()
elif ip == "3":
    markAttendance("raja")
else:
    print("Input not valid")