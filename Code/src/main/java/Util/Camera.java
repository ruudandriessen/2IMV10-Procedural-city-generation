/*
 * Copyright 2016 ruudandriessen.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package Util;

import static java.awt.image.ImageObserver.HEIGHT;
import static java.awt.image.ImageObserver.WIDTH;
import static java.lang.Math.PI;
import java.nio.FloatBuffer;
import org.lwjgl.BufferUtils;
import org.lwjgl.input.Keyboard;
import static org.lwjgl.opengl.GL20.glUniformMatrix4;
import static org.lwjgl.opengl.GL20.glUseProgram;
import org.lwjgl.util.vector.Matrix4f;
import org.lwjgl.util.vector.Vector3f;

/**
 *
 * @author ruudandriessen
 */
public class Camera {
    // Projection and view matrix
    private Matrix4f projectionMatrix = null;
    private Matrix4f viewMatrix = null;
    
    // Model stuff
    private Vector3f modelPos = null;
    private Vector3f modelAngle = null;
    private Vector3f modelScale = null;
    
    // Camera information
    private Vector3f cameraPos = null;
    private float yaw, pitch;
    private FloatBuffer matrix44Buffer;
    
    
    private int pId;
    private int projectionMatrixLocation;
    private int viewMatrixLocation;
    private int modelMatrixLocation;
    
    private final float moveAmount = 0.1f;
    private final float rotateAmount = 150f;
    
    private static final Vector3f xAxis = new Vector3f(1, 0, 0);
    private static final Vector3f yAxis = new Vector3f(0, 1, 0);
    private static final Vector3f zAxis = new Vector3f(0, 0, 1);

    
    public void create() {        
        // Setup view matrix
        viewMatrix = new Matrix4f();

        // Set the default quad rotation, scale and position values
        modelPos = new Vector3f(0, 0, 0);
        modelAngle = new Vector3f(0, 0, 0);
        modelScale = new Vector3f(1, 1, 1); // 42.868, 1.489
        
        cameraPos = new Vector3f(0, 0, -1);
        yaw = 0.0f;
        pitch = 0.0f;
        
        // Setup projection matrix
        projectionMatrix = new Matrix4f();
        float fieldOfView = 60f;
        float aspectRatio = (float)WIDTH / (float)HEIGHT;
        float near_plane = 0.1f;
        float far_plane = 100f;
         
        float y_scale = this.coTangent(this.degreesToRadians(fieldOfView / 2f));
        float x_scale = y_scale / aspectRatio;
        float frustum_length = far_plane - near_plane;
         
        projectionMatrix.m00 = x_scale;
        projectionMatrix.m11 = y_scale;
        projectionMatrix.m22 = -((far_plane + near_plane) / frustum_length);
        projectionMatrix.m23 = -1;
        projectionMatrix.m32 = -((2 * near_plane * far_plane) / frustum_length);
        projectionMatrix.m33 = 0; 
        
        // Setup view matrix
        viewMatrix = new Matrix4f();
         
        // Create a FloatBuffer with the proper size to store our matrices later
        matrix44Buffer = BufferUtils.createFloatBuffer(16);
    }
    
    public void apply() {    
        // Reset view and model matrices
        viewMatrix = new Matrix4f();
        
        // World matrix
        Matrix4f modelMatrix = new Matrix4f();
         
        // Translate camera
        Matrix4f.translate(cameraPos, viewMatrix, viewMatrix);
        Matrix4f.rotate(yaw, zAxis, viewMatrix, viewMatrix);
        Matrix4f.rotate(pitch, yAxis, viewMatrix, viewMatrix);
         
        // Scale, translate and rotate model
        Matrix4f.scale(modelScale, modelMatrix, modelMatrix);
        Matrix4f.translate(modelPos, modelMatrix, modelMatrix);
        Matrix4f.rotate(this.degreesToRadians(modelAngle.z), zAxis, modelMatrix, modelMatrix);
        Matrix4f.rotate(this.degreesToRadians(modelAngle.y), yAxis, modelMatrix, modelMatrix);
        Matrix4f.rotate(this.degreesToRadians(modelAngle.x), xAxis, modelMatrix, modelMatrix);
        
        glUseProgram(pId);
        
        // Upload matrices to the uniform variables         
        projectionMatrix.store(matrix44Buffer); matrix44Buffer.flip();
        glUniformMatrix4(projectionMatrixLocation, false, matrix44Buffer);
        viewMatrix.store(matrix44Buffer); matrix44Buffer.flip();
        glUniformMatrix4(viewMatrixLocation, false, matrix44Buffer);
        modelMatrix.store(matrix44Buffer); matrix44Buffer.flip();
        glUniformMatrix4(modelMatrixLocation, false, matrix44Buffer);
        
        glUseProgram(0);
    }
    
    public void processInput(float delta) {
        switch (Keyboard.getEventKey()) {
            case Keyboard.KEY_W:
                this.moveForward(delta);
            break;
            case Keyboard.KEY_S:
                this.moveBackward(delta);
            break; 
            case Keyboard.KEY_A:
                this.strafeLeft(delta);
            break;
            case Keyboard.KEY_D:
                this.strafeRight(delta);
            break;
        }
    }    
    
    public void setUniformLocations(int pId, int projectionMatrixLocation, int viewMatrixLocation, int modelMatrixLocation) {
        this.pId = pId;
        this.projectionMatrixLocation = projectionMatrixLocation;
        this.viewMatrixLocation = viewMatrixLocation;
        this.modelMatrixLocation = modelMatrixLocation;
    }
    
    private void rotateLeft(float delta) {
        yaw += delta * rotateAmount;
    }
    
    private void rotateRight(float delta) {
        yaw -= delta * rotateAmount;        
    }
           
    private void moveForward(float delta) {
        cameraPos.z += moveAmount * delta * Math.sin(Math.toRadians(cameraPos.y + 90));
        cameraPos.x += moveAmount * delta * Math.cos(Math.toRadians(cameraPos.y + 90));
    } 
    
    private void moveBackward(float delta) {
        cameraPos.z += moveAmount * delta * Math.sin(Math.toRadians(cameraPos.y + 90 * -1.0f));
        cameraPos.x += moveAmount * delta * Math.cos(Math.toRadians(cameraPos.y + 90 * -1.0f));
    }
    
    //strafes the camera left relitive to its current rotation (yaw)
    public void strafeLeft(float delta)
    {
        cameraPos.x -= moveAmount * delta * (float)Math.sin(Math.toRadians(yaw-90));
        cameraPos.z += moveAmount * delta * (float)Math.cos(Math.toRadians(yaw-90));
    }

    //strafes the camera right relitive to its current rotation (yaw)
    public void strafeRight(float delta)
    {
        cameraPos.x -= moveAmount * delta * (float)Math.sin(Math.toRadians(yaw+90));
        cameraPos.z += moveAmount * delta * (float)Math.cos(Math.toRadians(yaw+90));
    }
    
    private float coTangent(float angle) {
        return (float)(1f / Math.tan(angle));
    }
     
    private float degreesToRadians(float degrees) {
        return degrees * (float)(PI / 180d);
    } 
}
